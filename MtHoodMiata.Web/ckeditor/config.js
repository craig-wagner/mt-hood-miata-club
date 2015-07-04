/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {

    config.skin = 'office2003';
    config.contentsCss = '/Content/MtHoodMiata.css';
    config.height = '300px';
    config.resize_dir = 'vertical';
    config.resize_maxHeight = '500';

    config.forcePasteAsPlainText = true;

    config.language = 'en';

    config.toolbar = 'Custom';
    config.toolbar_Custom =
    [
        ['Preview', '-', 'SpellChecker'],
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord'],
        ['Undo', 'Redo'],['Source'],
        '/',
        ['Styles'],
        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Link', 'Unlink'],
        ['Image', 'Table', 'HorizontalRule']
    ];

    config.stylesSet = 'mhmcStyles:/Content/styles.js';
};
