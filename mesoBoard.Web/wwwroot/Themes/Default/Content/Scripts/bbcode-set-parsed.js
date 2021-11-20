// ----------------------------------------------------------------------------
// markItUp!
// ----------------------------------------------------------------------------
// Copyright (C) 2008 Jay Salvat
// http://markitup.jaysalvat.com/
// ----------------------------------------------------------------------------
// BBCode tags example
// http://en.wikipedia.org/wiki/Bbcode
// ----------------------------------------------------------------------------
// Feel free to add more tags
// ----------------------------------------------------------------------------
bbcodeSettings = {
    nameSpace: 'bbcode',
	previewParserPath:	'', // path to your BBCode parser
	markupSet: [
		{name:'Bold', key:'B', openWith:'[b]', closeWith:'[/b]'},
		{name:'Italic', key:'I', openWith:'[i]', closeWith:'[/i]'},
		{name:'Underline', key:'U', openWith:'[u]', closeWith:'[/u]'},
		{separator:'---------------' },
		{name:'Picture', key:'P', replaceWith:'[img][![Url]!][/img]'},
		{name:'Link', key:'L', openWith:'[url=[![Url]!]]', closeWith:'[/url]', placeHolder:'Your text to link here...'},
		{separator:'---------------' },
		{name:'Size', key:'S', openWith:'[size=[![Text size]!]]', closeWith:'[/size]',
		dropMenu :[
			{name:'Big', openWith:'[big]', closeWith:'[/big]' },
			{name:'Normal', openWith:'[normal]', closeWith:'[/normal]' },
			{name:'Small', openWith:'[small]', closeWith:'[/small]' }
		]},
		{separator:'---------------' },
		{name:'Bulleted list', openWith:'[list]\n', closeWith:'\n[/list]'},
		{name:'Numeric list', openWith:'[list=[![Starting number]!]]\n', closeWith:'\n[/list]'}, 
		{name:'List item', openWith:'[*] '},
		{separator:'---------------' },
		{ name: 'Quotes', openWith: '[quote=', closeWith: '][/quote]' },
		{ name: 'Syntax', className: 'syntax', openWith: '[code]', closeWith: '[/code]', dropMenu: [
				{ name: 'C#', openWith: '[code=csharp]', closeWith: '[/code]' },
				{ name: 'PHP', openWith: '[code=php]', closeWith: '[/code]' },
				{ name: 'JScript', openWith: '[code=jscript]', closeWith: '[/code]' },
				{ name: 'CSS', openWith: '[code=css]', closeWith: '[/code]' },
				{ name: 'SQL', openWith: '[code=sql]', closeWith: '[/code]' },
				{ name: 'VB', openWith: '[code=vb]', closeWith: '[/code]' },
				{ name: 'XML', openWith: '[code=xml]', closeWith: '[/code]' },
				{ name: 'Java', openWith: '[code=java]', closeWith: '[/code]' },
				{ name: 'C++', openWith: '[code=cpp]', closeWith: '[/code]' },
				{ name: 'Delphi', openWith: '[code=delphi]', closeWith: '[/code]' },
				{ name: 'Ruby', openWith: '[code=ruby]', closeWith: '[/code]' },
				{ name: 'Python', openWith: '[code=python]', closeWith: '[/code]' }
		]
		},
		{name:'Colors', className:'palette', dropMenu: [
				{name:'Yellow',	openWith:'[color=#FCE94F]', closeWith:'[/color]', className:"col1-1" },
				{name:'Yellow',	openWith:'[color=#EDD400]', closeWith:'[/color]', className:"col1-2" },
				{name:'Yellow', openWith:'[color=#C4A000]', closeWith:'[/color]', className:"col1-3" },
				
				{name:'Orange', openWith:'[color=#FCAF3E]', closeWith:'[/color]', className:"col2-1" },
				{name:'Orange', openWith:'[color=#F57900]', closeWith:'[/color]', className:"col2-2" },
				{name:'Orange', openWith:'[color=#CE5C00]', closeWith:'[/color]', className:"col2-3" },
				
				{name:'Brown', 	openWith:'[color=#E9B96E]', closeWith:'[/color]', className:"col3-1" },
				{name:'Brown', 	openWith:'[color=#C17D11]', closeWith:'[/color]', className:"col3-2" },
				{name:'Brown',	openWith:'[color=#8F5902]', closeWith:'[/color]', className:"col3-3" },
				
				{name:'Green', 	openWith:'[color=#8AE234]', closeWith:'[/color]', className:"col4-1" },
				{name:'Green', 	openWith:'[color=#73D216]', closeWith:'[/color]', className:"col4-2" },
				{name:'Green',	openWith:'[color=#4E9A06]', closeWith:'[/color]', className:"col4-3" },
				
				{name:'Blue', 	openWith:'[color=#729FCF]', closeWith:'[/color]', className:"col5-1" },
				{name:'Blue', 	openWith:'[color=#3465A4]', closeWith:'[/color]', className:"col5-2" },
				{name:'Blue',	openWith:'[color=#204A87]', closeWith:'[/color]', className:"col5-3" },
	
				{name:'Purple', openWith:'[color=#AD7FA8]', closeWith:'[/color]', className:"col6-1" },
				{name:'Purple', openWith:'[color=#75507B]', closeWith:'[/color]', className:"col6-2" },
				{name:'Purple',	openWith:'[color=#5C3566]', closeWith:'[/color]', className:"col6-3" },
				
				{name:'Red', 	openWith:'[color=#EF2929]', closeWith:'[/color]', className:"col7-1" },
				{name:'Red', 	openWith:'[color=#CC0000]', closeWith:'[/color]', className:"col7-2" },
				{name:'Red',	openWith:'[color=#A40000]', closeWith:'[/color]', className:"col7-3" },
				
				{name:'White', 	openWith:'[color=#FFFFFF]', closeWith:'[/color]', className:"col8-1" },
				{name:'Gray', 	openWith:'[color=#D3D7CF]', closeWith:'[/color]', className:"col8-2" },
				{name:'Gray',	openWith:'[color=#BABDB6]', closeWith:'[/color]', className:"col8-3" },
				
				{name:'Gray', 	openWith:'[color=#888A85]', closeWith:'[/color]', className:"col9-1" },
				{name:'Gray', 	openWith:'[color=#555753]', closeWith:'[/color]', className:"col9-2" },
				{name:'Black',	openWith:'[color=#000000]', closeWith:'[/color]', className:"col9-3" }
			]
		}		
	]
}