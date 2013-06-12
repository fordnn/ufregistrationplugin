<%@ Control Language="C#" Inherits="forDNN.Modules.ufRegistrationPlugin.ViewufRegistrationPlugin"
	AutoEventWireup="true" CodeBehind="ufRegistrationPlugin.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div class="fieldSetDiv">
	<div class="controlDiv">
		<div class="lLabel">
			<dnn:label resourcekey="VerifyUserName" id="lblVerifyUserName" runat="server" associatedcontrolid="cbVerifyUserName">
			</dnn:label>
		</div>
		<div class="rControl">
			<input type="checkbox" id="cbVerifyUserName" runat="server" data-bind="checked:verifyUserName" />
		</div>
	</div>
	<div class="controlDiv" data-bind="visible:verifyUserName">
		<div class="lLabel">
			<dnn:label resourcekey="SuggestUserName" id="lblSuggestUserName" runat="server" associatedcontrolid="cbSuggestUserName">
			</dnn:label>
		</div>
		<div class="rControl">
			<input type="checkbox" id="cbSuggestUserName" runat="server" data-bind="checked:suggestUserName" />
		</div>
	</div>
	<div class="controlDiv" data-bind="visible:suggestionTemplatesEnabled">
		<div class="lLabel">
			<dnn:label resourcekey="SuggestionTemplates" id="lblSuggestionTemplates" runat="server"
				associatedcontrolid="tbSuggestionTemplates">
			</dnn:label>
		</div>
		<div class="rControl">
			<asp:TextBox runat="server" ID="tbSuggestionTemplates" TextMode="MultiLine" CssClass="suggestionTemplates"></asp:TextBox>
			<asp:LinkButton ID="btnRestoreDefaultTemplates" runat="server" CssClass="CommandButton"
				resourcekey="RestoreDefaultTemplates" OnClick="btnRestoreDefaultTemplates_Click"></asp:LinkButton>
		</div>
	</div>
	<div class="controlDiv">
		<div class="lLabel">
			<dnn:label resourcekey="VerifyEmail" id="lblVerifyEmail" runat="server" associatedcontrolid="cbVerifyEmail">
			</dnn:label>
		</div>
		<div class="rControl">
			<input type="checkbox" id="cbVerifyEmail" runat="server" />
		</div>
	</div>
	<div class="controlDiv">
		<div class="lLabel">
			<dnn:label resourcekey="VerifyPassword" id="lblVerifyPassword" runat="server" associatedcontrolid="cbVerifyPassword">
			</dnn:label>
		</div>
		<div class="rControl">
			<input type="checkbox" id="cbVerifyPassword" runat="server" />
		</div>
	</div>
	<div class="controlDiv">
		<asp:LinkButton runat="server" ID="btnSave" resourcekey="Save" CssClass="dnnPrimaryAction"
			OnClick="btnSave_Click"></asp:LinkButton>&nbsp;
		<asp:LinkButton runat="server" ID="btnActivatePlugin" CssClass="dnnSecondaryAction" OnClick="btnActivatePlugin_Click"></asp:LinkButton>
	</div>
</div>

<script type="text/javascript">
	function viewModel()
	{
		this.verifyUserName = ko.observable($("#<%=cbVerifyUserName.ClientID%>").is(":checked"));
		this.suggestUserName = ko.observable($("#<%=cbSuggestUserName.ClientID%>").is(":checked"));
		this.suggestionTemplatesEnabled = ko.computed(function()
		{
			return this.verifyUserName() && this.suggestUserName();
		}, this);
	};

	$(document).ready(function()
	{
		ko.applyBindings(new viewModel());
	});
</script>

