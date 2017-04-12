"use strict";
// 测试枚举
var TestEnum;
(function (TestEnum) {
    // A值
    TestEnum[TestEnum["A"] = 1] = "A";
    // B值
    TestEnum[TestEnum["B"] = 3] = "B";
    // C值
    TestEnum[TestEnum["C"] = 5] = "C";
})(TestEnum = exports.TestEnum || (exports.TestEnum = {}));
