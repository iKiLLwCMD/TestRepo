/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config )
{
	config.toolbar = 'MyToolbar';
 
	config.toolbar_MyToolbar =
	[
		{ name: 'document', items : [ 'Image', 'NewPage','Preview' ] },
		{ name: 'clipboard', items : [ 'Cut','Copy','Paste','PasteText','PasteFromWord','-','Undo','Redo' ] },
		{ name: 'source', items : [ 'Source' ] },
		{ name: 'styles', items : [ 'Styles','Format', 'Font' ] },
		{ name: 'basicstyles', items : [ 'Bold','Italic','Strike','-','RemoveFormat' ] },
		{ name: 'paragraph', items : [ 'NumberedList','BulletedList','-','Outdent','Indent','-','Blockquote' ] },
		{ name: 'links', items : [ 'Link','Unlink','Anchor' ] },
		{ name: 'tools', items : [ 'Maximize','strinsert' ] }
	];
	
	config.skin = 'moonocolor';
	config.extraPlugins = 'strinsert';
};

CKEDITOR.replace( 'editor1',
	{
		toolbar : 'MyToolbar'
	});
 
CKEDITOR.replace( 'editor2',
	{
		toolbar : 'Basic'
	});
