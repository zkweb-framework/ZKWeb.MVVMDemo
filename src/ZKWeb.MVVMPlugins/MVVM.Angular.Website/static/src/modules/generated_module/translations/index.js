"use strict";
var zh_cn_1 = require("./zh-cn");
var en_us_1 = require("./en-us");
var TranslationIndex = (function () {
    function TranslationIndex() {
    }
    return TranslationIndex;
}());
TranslationIndex.translationModules = [
    zh_cn_1.Translation_zh_CN,
    en_us_1.Translation_en_US
];
exports.TranslationIndex = TranslationIndex;
