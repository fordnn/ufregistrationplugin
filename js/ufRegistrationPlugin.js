var ufRegistrationPlugin =
		{
			ctlParent: "",
			scriptURL: "",
			verifyingMessage: "Verifying...",
			ctlFirstName: "",
			ctlLastName: "",
			ctlUserName: "",
			ctlEmail: "",
			ctlPassword: "",

			//const
			ctlUserNameSuggestion: "#divUserNameSuggestion",
			ctlEmailVerification: "#divEmailVerification",
			ctlPasswordVerification: "#divPasswordVerification",

			//settings
			verifyUserName: false,
			suggestUserName: false,
			verifyEmail: false,
			verifyPassword: false,

			initPlugin: function(_ctlParent, _scriptURL)
			{
				ufRegistrationPlugin.ctlParent = _ctlParent;
				ufRegistrationPlugin.scriptURL = _scriptURL + "?action=";

				//load necessary css
				var cssURL = ufRegistrationPlugin.scriptURL.toLowerCase().replace("ufregistrationscript.aspx", "ufRegistrationPlugin.css");
				if (document.createStyleSheet)
				{
					document.createStyleSheet(cssURL);
				}
				else
				{
					$("head").append($("<link rel='stylesheet' href='" + cssURL + "' type='text/css' />"));
				}

				//add handler to the "Register" button
				var ctlRegister = ufRegistrationPlugin.ctlParent.replace("userForm", "registerButton");
				$("#" + ctlRegister).click(function() { return ufRegistrationSuccess(); });

				//controls
				var subName = this.ctlParent.replace(/_/g, "$");

				ufRegistrationPlugin.ctlFirstName = '[name="' + subName + '$FirstName$FirstName_Control"]';
				ufRegistrationPlugin.ctlLastName = '[name="' + subName + '$LastName$LastName_Control"]';
				ufRegistrationPlugin.ctlUserName = '[name="' + subName + '$Username$Username_TextBox"]';
				ufRegistrationPlugin.ctlEmail = '[name="' + subName + '$Email$Email_TextBox"]';
				ufRegistrationPlugin.ctlPassword = '[name="' + subName + '$Password$Password_TextBox"]';

				$.ajax({
					type: "POST",
					async: true,
					url: ufRegistrationPlugin.scriptURL + "getsettings",
					data: null,
					contentType: "application/json",
					dataType: "json",
					success: function(msg)
					{
						ufRegistrationPlugin.verifyUserName = msg.VerifyUserName;
						ufRegistrationPlugin.suggestUserName = msg.SuggestUserName;
						ufRegistrationPlugin.verifyEmail = msg.VerifyEmail;
						ufRegistrationPlugin.verifyPassword = msg.VerifyPassword;
						ufRegistrationPlugin.verifyingMessage = msg.VerifyingMessage;

						if (ufRegistrationPlugin.verifyUserName)
						{
							$(ufRegistrationPlugin.ctlUserName).blur(function() { verifyUserName(); });
						}

						if (ufRegistrationPlugin.verifyEmail)
						{
							$(ufRegistrationPlugin.ctlEmail).blur(function() { verifyEmail(); });
						}

						if (ufRegistrationPlugin.verifyPassword)
						{
							$(ufRegistrationPlugin.ctlPassword).blur(function() { verifyPassword(); });
						}
					}
				});
			}
		};

function makeCall(action, params, destination)
{
	$.ajax({
		type: "POST",
		async: true,
		url: ufRegistrationPlugin.scriptURL + action,
		data: params,
		contentType: "application/json",
		dataType: "json",
		success: function(msg)
		{
			$(destination).html("");
			if (msg != null)
			{
				if (msg.success == true)
				{
					if (destination != "")
					{
						$(destination).html(msg.innerHTML);
					}
				}
			}
			if ($(destination).html() == "")
			{
				$(destination).parent().hide();
			}
			else
			{
				$(destination).parent().show();
			}
		},
		error: function(xmlRequest, textStatus, errorThrown)
		{
			$(destination).html("");
		}
	});
	return false;
}

function ctlExists(ctlID, ctlInsertAfter)
{
	if ($(ctlID).length > 0)
	{
		return;
	}
	$(ctlInsertAfter).parent().after("<div><div class=\"dnnTooltip\">&nbsp;</div><div id=\"" + ctlID.replace("#", "") + "\" vld=\"1\" class=\"verifycationResponse\"></div></div>");
}

function verifyUserName()
{
	ctlExists(ufRegistrationPlugin.ctlUserNameSuggestion, ufRegistrationPlugin.ctlUserName);
	$(ufRegistrationPlugin.ctlUserNameSuggestion).html(ufRegistrationPlugin.verifyingMessage);

	var objRequest = new Object();
	objRequest.firstName = $(ufRegistrationPlugin.ctlFirstName).val();
	objRequest.lastName = $(ufRegistrationPlugin.ctlLastName).val();
	objRequest.userName = $(ufRegistrationPlugin.ctlUserName).val();

	makeCall("checkusername", objRequest, ufRegistrationPlugin.ctlUserNameSuggestion);
}

function verifyEmail()
{
	ctlExists(ufRegistrationPlugin.ctlEmailVerification, ufRegistrationPlugin.ctlEmail);
	$(ufRegistrationPlugin.ctlEmailVerification).html(ufRegistrationPlugin.verifyingMessage);

	var objRequest = new Object();
	objRequest.email = $(ufRegistrationPlugin.ctlEmail).val();

	makeCall("verifyemail", objRequest, ufRegistrationPlugin.ctlEmailVerification);
}

function selectSuggestion(suggestion)
{
	$(ufRegistrationPlugin.ctlUserName).val(suggestion);
	$(ufRegistrationPlugin.ctlUserNameSuggestion).html("");
	$(ufRegistrationPlugin.ctlUserNameSuggestion).parent().hide();
	return false;
}

function verifyPassword()
{
	ctlExists(ufRegistrationPlugin.ctlPasswordVerification, ufRegistrationPlugin.ctlPassword);
	$(ufRegistrationPlugin.ctlPasswordVerification).html(ufRegistrationPlugin.verifyingMessage);

	var objRequest = new Object();
	objRequest.password = $(ufRegistrationPlugin.ctlPassword).val();

	makeCall("verifypassword", objRequest, ufRegistrationPlugin.ctlPasswordVerification);
}

function ufRegistrationSuccess()
{
	var lstValidators = $('div[vld="1"]');
	for (var i = 0; i < lstValidators.length; i++)
	{
		if ($(lstValidators[i]).html() != "")
		{
			return false;
		}
	}
	return true;
}
