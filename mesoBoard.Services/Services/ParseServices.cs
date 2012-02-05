using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using mesoBoard.Common;
using mesoBoard.Data;
using System.Web.Mvc;

namespace mesoBoard.Services
{
    public class ParseServices : BaseService
    {
        string _bbCodePattern = @"\[(\w+)\b([^\]]*)\](.*?)\[/\1\]";
        IRepository<BBCode> _bbCodeRepository;
        IRepository<Smiley> _smilieRepository;

        public ParseServices(
            IRepository<BBCode> bbCodeRepository, 
            IRepository<Smiley> smilieRepository,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _bbCodeRepository = bbCodeRepository;
            _smilieRepository = smilieRepository;
        }

        public string GetTextOnly(string text)
        {
            string pattern = @"\[(.|\n)*?\]";
            return Regex.Replace(text, pattern, string.Empty);
        }

        /// <summary>
        /// HTML encodes a string then parses BB codes.
        /// </summary>
        /// <param name="text">The string to HTML encode anad BB code parse</param>
        /// <returns></returns>
        public string ParseBBCodeText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = HttpUtility.HtmlEncode(text);
            text = text.Replace(Environment.NewLine, "<br />");

            Regex matcher = new Regex(this._bbCodePattern, RegexOptions.Compiled);

            Match found = matcher.Match(text);
            while (found.Success)
            {
                string tag = found.Groups[1].Value;
                BBCode code = _bbCodeRepository.First(item => item.Tag == tag);
                if (code == null)
                {
                    // break the Regex match to avoid infinite loops and find next match
                    string replaceWith = found.Value.Replace("[", "^-^-^").Replace("]", "%-%-%");
                    text = text.Replace(found.Value, replaceWith);
                }
                else
                {
                    string parsedText = string.Empty;
                    string secondCaptureGroup = found.Groups[3].Value;

                    if (code.Parse.Contains("{2}"))
                    {
                        string firstCaptureGroup = found.Groups[2].Value;

                        // Gets rid of the '=' caught by the Regex in the first capture group
                        if (!string.IsNullOrWhiteSpace(firstCaptureGroup))
                            firstCaptureGroup = found.Groups[2].Value.Substring(1, found.Groups[2].Value.Length - 1);

                        if (string.IsNullOrWhiteSpace(secondCaptureGroup))
                            secondCaptureGroup = firstCaptureGroup;

                        parsedText = string.Format(code.Parse, "", firstCaptureGroup, secondCaptureGroup);
                    }
                    else
                    {
                        parsedText = string.Format(code.Parse, "", secondCaptureGroup);
                    }

                    if (code.Tag.Equals("code"))
                        parsedText = parsedText.Replace("<br />", Environment.NewLine);

                    text = text.Replace(found.Value, parsedText);
                }

                found = matcher.Match(text);
            }

            // undo the replace that was used to break Regex to avoid infinite loops
            text = text.Replace("^-^-^", "[").Replace("%-%-%", "]");

            TagBuilder smileyImage = new TagBuilder("img");
            smileyImage.Attributes.Add("src", string.Empty);
            smileyImage.Attributes.Add("alt", string.Empty);
            smileyImage.Attributes.Add("title", string.Empty);

            foreach (Smiley smiley in _smilieRepository.Get().ToList())
            {
                string imageUrl = string.Format("{0}/{1}", DirectoryPaths.Smilies.TrimStart('~').TrimEnd('/'), smiley.ImageURL);
                smileyImage.Attributes["src"] = imageUrl;
                smileyImage.Attributes["alt"] = smiley.Title;
                smileyImage.Attributes["title"] = smiley.Title;
                text = text.Replace(smiley.Code, smileyImage.ToString(TagRenderMode.SelfClosing));
            }
            return text;
        }
    }
}
