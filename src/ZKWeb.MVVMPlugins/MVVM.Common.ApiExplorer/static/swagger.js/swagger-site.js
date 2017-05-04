var SwaggerSiteInfo = {
	sessionId: ""
};

(function () {
	// 让服务端认为请求时ajax请求
	window.swaggerUi.api.clientAuthorizations.add("XRequestWith",
		new SwaggerClient.ApiKeyAuthorization("X-Requested-With", "XMLHttpRequest", "header"));
	// 注册全局ajax处理器
	$(document).ajaxSuccess(function (e, xhr) {
		var sessionId = xhr.getResponseHeader("X-Set-ZKWeb-SessionId");
		if (sessionId) {
			console.log("set session id", sessionId);
			SwaggerSiteInfo.sessionId = sessionId;
			// 让swagger添加自定义的http头
			window.swaggerUi.api.clientAuthorizations.remove("XZKWebSessionId");
			window.swaggerUi.api.clientAuthorizations.add("XZKWebSessionId",
				new SwaggerClient.ApiKeyAuthorization("X-ZKWeb-SessionId", sessionId, "header"));
		}
	});
	$(document).ajaxSend(function (e, xhr) {
		xhr.setRequestHeader("X-ZKWeb-SessionId", SwaggerSiteInfo.sessionId);
		xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
	});
	// 创建登录弹出框
	var $form = $("<div>", { "class": "container" })
		.css({
			position: "fixed",
			left: "0px",
			top: "0px",
			width: "100%",
			height: "100%",
			display: "flex",
			"align-items": "center",
			"justify-content": "center",
			background: "rgba(0, 0, 0, 0.5)"
		})
		.append($("<div>", { "class": "form" })
			.css({ background: "#fff", padding: "20px" })
			.append($("<div>").append($("<span>").text("Tenant: ")).append($("<input>", { name: "Tenant", type: "text", value: "Master" })))
			.append($("<div>").append($("<span>").text("Username: ")).append($("<input>", { name: "Username", type: "text", value: "" })))
			.append($("<div>").append($("<span>").text("Password: ")).append($("<input>", { name: "Password", type: "password", value: "" })))
			.append($("<div>", { "class": "captcha" })
				.append($("<span>").text("Captcha: "))
				.append($("<input>", { name: "Captcha", value: "" }))
				.append($("<img>", { src: "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=", alt: "Loading" })))
			.append($("<div>").append($("<input>", { "class": "submit", type: "submit", value: "login" }).css({ width: "100%" }))));
	$("body").append($form);
	// 获取验证码
	var refreshCaptcha = function () {
		var args = { key: "AdminLogin" };
		$.post("/api/CaptchaService/GetCaptchaImageBase64", args)
			.done(function (result) {
				$form.find(".captcha > img").attr("src", "data:image/png;base64," + result);
			}).fail(function (xhr) {
				alert(xhr.responseText);
			});
	};
	refreshCaptcha();
	// 绑定登录事件
	$form.find(".submit").click(function () {
		var tenant = $form.find("[name=Tenant]").val();
		var username = $form.find("[name=Username]").val();
		var password = $form.find("[name=Password]").val();
		var captcha = $form.find("[name=Captcha]").val();
		var args = {
			Tenant: tenant,
			Username: username,
			Password: password,
			Captcha: captcha
		};
		$.post("/api/UserLoginService/LoginAdmin", args)
			.done(function (result) {
				alert("Login Success");
				$form.hide();
			}).fail(function (xhr) {
				refreshCaptcha();
				alert(xhr.responseText);
			});
		return false;
	});
})();
