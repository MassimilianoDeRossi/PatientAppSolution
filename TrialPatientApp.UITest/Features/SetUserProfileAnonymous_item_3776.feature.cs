﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace PatientApp.UITest.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("SetUserProfileAnonymous_item_3776")]
    public partial class SetUserProfileAnonymous_Item_3776Feature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SetUserProfileAnonymous_item_3776.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "SetUserProfileAnonymous_item_3776", "In order to set user profile\r\nAs an anonymous user of myHEXplan app\r\nI want to co" +
                    "nfigure User Profile settings\r\nRules:\r\nnickname max 20 chars", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap and confirm exit option in Welcome User Anonymous page")]
        public virtual void UserTapAndConfirmExitOptionInWelcomeUserAnonymousPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap and confirm exit option in Welcome User Anonymous page", ((string[])(null)));
#line 10
this.ScenarioSetup(scenarioInfo);
#line 11
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 12
 testRunner.And("User navigate to \'Welcome User Anonymous\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.When("User tap on \'Exit\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
 testRunner.And("User \'confirm\' to Exit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.Then("\'Home Anonymous\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap but not confirm exit option in Welcome User Anonymous page")]
        public virtual void UserTapButNotConfirmExitOptionInWelcomeUserAnonymousPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap but not confirm exit option in Welcome User Anonymous page", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
 testRunner.And("User navigate to \'Welcome User Anonymous\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
 testRunner.When("User tap on \'Exit\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
 testRunner.And("User \'not confirm\' to Exit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.Then("\'Welcome User Anonymous\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap next option in Welcome User Anonymous page")]
        public virtual void UserTapNextOptionInWelcomeUserAnonymousPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap next option in Welcome User Anonymous page", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 25
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
 testRunner.And("User navigate to \'Welcome User Anonymous\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.When("User tap on \'Next\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.Then("\'User Profile\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap back option in User Profile page")]
        public virtual void UserTapBackOptionInUserProfilePage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap back option in User Profile page", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 33
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 34
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.When("User tap on \'Back\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 36
 testRunner.Then("\'Welcome User Anonymous\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap next option in User Profile page")]
        public virtual void UserTapNextOptionInUserProfilePage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap next option in User Profile page", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line 39
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 40
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
 testRunner.When("User tap on \'Next\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 42
 testRunner.Then("\'Pin Site Care\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap and confirm exit option in User Profile page")]
        public virtual void UserTapAndConfirmExitOptionInUserProfilePage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap and confirm exit option in User Profile page", ((string[])(null)));
#line 44
this.ScenarioSetup(scenarioInfo);
#line 45
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 46
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
 testRunner.When("User tap on \'Exit\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 48
 testRunner.And("User \'confirm\' to Exit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
 testRunner.Then("\'Home Anonymous\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap but not confirm exit option in User Profile page")]
        public virtual void UserTapButNotConfirmExitOptionInUserProfilePage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap but not confirm exit option in User Profile page", ((string[])(null)));
#line 51
this.ScenarioSetup(scenarioInfo);
#line 52
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 53
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
 testRunner.When("User tap on \'Exit\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 55
 testRunner.And("User \'not confirm\' to Exit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
 testRunner.Then("\'User Profile\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap exit option in User Profile page with data entered")]
        [NUnit.Framework.TestCaseAttribute("\'Davide\'", null)]
        [NUnit.Framework.TestCaseAttribute("\'Diego\'", null)]
        [NUnit.Framework.TestCaseAttribute("\'empty\'", null)]
        public virtual void UserTapExitOptionInUserProfilePageWithDataEntered(string myname, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap exit option in User Profile page with data entered", exampleTags);
#line 58
this.ScenarioSetup(scenarioInfo);
#line 59
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 60
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
 testRunner.When(string.Format("User enter \'nickname\' with {0}", myname), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 62
 testRunner.And("User tap on \'Exit\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
 testRunner.And("User \'confirm\' to Exit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.And("User navigate to \'User Profile\' page from \'Home Anonymous\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.Then("The \'nickname\' should be \'empty\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify default values in User Profile page")]
        public virtual void VerifyDefaultValuesInUserProfilePage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify default values in User Profile page", ((string[])(null)));
#line 72
this.ScenarioSetup(scenarioInfo);
#line 73
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 74
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.Then("The \'nickname\' should be \'empty\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 76
 testRunner.And("The \'photo\' should be \'empty\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User wants to set nickname")]
        [NUnit.Framework.TestCaseAttribute("\'Davide\'", null)]
        [NUnit.Framework.TestCaseAttribute("\'Diego\'", null)]
        [NUnit.Framework.TestCaseAttribute("\'empty\'", null)]
        [NUnit.Framework.TestCaseAttribute("\'§§Tizio -.-\" °!\'", null)]
        public virtual void UserWantsToSetNickname(string myname, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User wants to set nickname", exampleTags);
#line 78
this.ScenarioSetup(scenarioInfo);
#line 79
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 80
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
 testRunner.When(string.Format("User enter \'nickname\' with {0}", myname), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 82
 testRunner.Then(string.Format("The \'nickname\' should be {0}", myname), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User wants to set nickname with special <>%#&? chars")]
        public virtual void UserWantsToSetNicknameWithSpecialChars()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User wants to set nickname with special <>%#&? chars", ((string[])(null)));
#line 90
this.ScenarioSetup(scenarioInfo);
#line 91
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 92
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
 testRunner.When("User enter \'nickname\' with \'<>%#&?\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 94
 testRunner.Then("The \'nickname\' should be \'empty\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User wants to set nickname with more than 20 chars")]
        public virtual void UserWantsToSetNicknameWithMoreThan20Chars()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User wants to set nickname with more than 20 chars", ((string[])(null)));
#line 96
this.ScenarioSetup(scenarioInfo);
#line 97
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 98
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
 testRunner.When("User enter \'nickname\' with \'12345678901234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 100
 testRunner.And("User tap on \'a char\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
 testRunner.Then("The \'nickname\' should be \'12345678901234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap back option in Pin Site Care page with data entered")]
        [NUnit.Framework.TestCaseAttribute("\'Davide123\'", null)]
        [NUnit.Framework.TestCaseAttribute("\'empty\'", null)]
        public virtual void UserTapBackOptionInPinSiteCarePageWithDataEntered(string nickname, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap back option in Pin Site Care page with data entered", exampleTags);
#line 105
this.ScenarioSetup(scenarioInfo);
#line 106
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 107
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
 testRunner.When(string.Format("User enter \'nickname\' with {0}", nickname), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 109
 testRunner.And("User navigate to \'Pin Site Care\' page from \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
 testRunner.And("User tap on \'Back\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
 testRunner.Then("\'User Profile\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 112
 testRunner.And(string.Format("The \'nickname\' should be {0}", nickname), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap and confirm exit option in Pin Site Care page")]
        public virtual void UserTapAndConfirmExitOptionInPinSiteCarePage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap and confirm exit option in Pin Site Care page", ((string[])(null)));
#line 118
this.ScenarioSetup(scenarioInfo);
#line 119
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 120
 testRunner.And("User navigate to \'Pin Site Care\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
 testRunner.When("User tap on \'Exit\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 122
 testRunner.And("User \'confirm\' to Exit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
 testRunner.Then("\'Home Anonymous\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap but not confirm exit option in Pin Site Care page")]
        public virtual void UserTapButNotConfirmExitOptionInPinSiteCarePage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap but not confirm exit option in Pin Site Care page", ((string[])(null)));
#line 125
this.ScenarioSetup(scenarioInfo);
#line 126
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 127
 testRunner.And("User navigate to \'Pin Site Care\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
 testRunner.When("User tap on \'Exit\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 129
 testRunner.And("User \'not confirm\' to Exit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
 testRunner.Then("\'Pin Site Care\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User tap exit option in Pin Site Care page with data entered")]
        [NUnit.Framework.TestCaseAttribute("\'Enrico\'", null)]
        [NUnit.Framework.TestCaseAttribute("\'empty\'", null)]
        public virtual void UserTapExitOptionInPinSiteCarePageWithDataEntered(string nickname, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User tap exit option in Pin Site Care page with data entered", exampleTags);
#line 132
this.ScenarioSetup(scenarioInfo);
#line 133
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 134
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
 testRunner.When(string.Format("User enter \'nickname\' with {0}", nickname), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 136
 testRunner.And("User navigate to \'Pin Site Care\' page from \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
 testRunner.And("User tap on \'Exit\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
 testRunner.And("User \'confirm\' to Exit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
 testRunner.And("User navigate to \'User Profile\' page from \'Home Anonymous\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
 testRunner.Then("The \'nickname\' should be \'empty\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify default values in Pin Site Care page")]
        public virtual void VerifyDefaultValuesInPinSiteCarePage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify default values in Pin Site Care page", ((string[])(null)));
#line 146
this.ScenarioSetup(scenarioInfo);
#line 147
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 148
 testRunner.And("User navigate to \'Pin Site Care\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 149
 testRunner.Then("The \'set daily time\' should be \'9:00 AM\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Successful profile settings using default values")]
        public virtual void SuccessfulProfileSettingsUsingDefaultValues()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Successful profile settings using default values", ((string[])(null)));
#line 151
this.ScenarioSetup(scenarioInfo);
#line 152
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 153
 testRunner.And("User navigate to \'Pin Site Care\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 154
 testRunner.When("User tap on \'Done\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 155
 testRunner.Then("\'Home Anonymous\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table1.AddRow(new string[] {
                        "empty",
                        "9:00 AM"});
#line 156
 testRunner.And("Anonymous Profile settings are successfully saved", ((string)(null)), table1, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Successful profile settings using not default values")]
        public virtual void SuccessfulProfileSettingsUsingNotDefaultValues()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Successful profile settings using not default values", ((string[])(null)));
#line 160
this.ScenarioSetup(scenarioInfo);
#line 161
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table2.AddRow(new string[] {
                        "MyNickname",
                        "18:40"});
#line 162
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table2, "When ");
#line 165
 testRunner.Then("\'Home Anonymous\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table3.AddRow(new string[] {
                        "MyNickname",
                        "6:40 PM"});
#line 166
 testRunner.And("Anonymous Profile settings are successfully saved", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Successful profile settings update")]
        public virtual void SuccessfulProfileSettingsUpdate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Successful profile settings update", ((string[])(null)));
#line 170
this.ScenarioSetup(scenarioInfo);
#line 171
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table4.AddRow(new string[] {
                        "Mynickname",
                        "15:45"});
#line 172
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table4, "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table5.AddRow(new string[] {
                        "newNick",
                        "17:32"});
#line 175
 testRunner.When("User \'update\' profile with", ((string)(null)), table5, "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table6.AddRow(new string[] {
                        "newNick",
                        "5:32 PM"});
#line 178
 testRunner.Then("Anonymous Profile settings are successfully saved", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify correct format hour in AM/PM anonymous user")]
        [NUnit.Framework.TestCaseAttribute("\'10:00\'", "\'AM\'", null)]
        [NUnit.Framework.TestCaseAttribute("\'03:50\'", "\'AM\'", null)]
        [NUnit.Framework.TestCaseAttribute("\'17:34\'", "\'PM\'", null)]
        [NUnit.Framework.TestCaseAttribute("\'23:43\'", "\'PM\'", null)]
        public virtual void VerifyCorrectFormatHourInAMPMAnonymousUser(string hour, string format, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify correct format hour in AM/PM anonymous user", exampleTags);
#line 182
this.ScenarioSetup(scenarioInfo);
#line 183
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 184
 testRunner.And("User navigate to \'Pin Site Care\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 185
 testRunner.When(string.Format("User enter \'set daily time\' with {0}", hour), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 186
 testRunner.Then(string.Format("The \'set daily time\' should be {0}", format), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User want to change set daily time N times")]
        public virtual void UserWantToChangeSetDailyTimeNTimes()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User want to change set daily time N times", ((string[])(null)));
#line 194
this.ScenarioSetup(scenarioInfo);
#line 195
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 196
 testRunner.And("User navigate to \'Pin Site Care\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 197
 testRunner.When("User enter \'set daily time\' with \'06:20\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 198
 testRunner.Then("The \'set daily time\' should be \'6:20 AM\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 199
 testRunner.When("User enter \'set daily time\' with \'18:40\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 200
 testRunner.Then("The \'set daily time\' should be \'6:40 PM\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 201
 testRunner.When("User enter \'set daily time\' with \'13:02\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 202
 testRunner.Then("The \'set daily time\' should be \'1:02 PM\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 203
 testRunner.When("User enter \'set daily time\' with \'10:58\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 204
 testRunner.Then("The \'set daily time\' should be \'10:58 AM\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 205
 testRunner.When("User enter \'set daily time\' with \'23:44\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 206
 testRunner.Then("The \'set daily time\' should be \'11:44 PM\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 207
 testRunner.When("User enter \'set daily time\' with \'21:07\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 208
 testRunner.Then("The \'set daily time\' should be \'9:07 PM\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 209
 testRunner.When("User enter \'set daily time\' with \'01:33\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 210
 testRunner.Then("The \'set daily time\' should be \'1:33 AM\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Swiping right with data entered in User Profile not lost data")]
        public virtual void SwipingRightWithDataEnteredInUserProfileNotLostData()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Swiping right with data entered in User Profile not lost data", ((string[])(null)));
#line 214
this.ScenarioSetup(scenarioInfo);
#line 215
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 216
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 217
 testRunner.When("User enter \'nickname\' with \'myNick\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 218
 testRunner.And("User swipe from \'left\' to \'right\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 219
 testRunner.And("User swipe from \'right\' to \'left\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 220
 testRunner.Then("The \'nickname\' should be \'myNick\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Swiping right with data entered in Pin Site Care not lost data")]
        public virtual void SwipingRightWithDataEnteredInPinSiteCareNotLostData()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Swiping right with data entered in Pin Site Care not lost data", ((string[])(null)));
#line 222
this.ScenarioSetup(scenarioInfo);
#line 223
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 224
 testRunner.And("User navigate to \'Pin Site Care\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 225
 testRunner.When("User enter \'set daily time\' with \'06:20\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 226
 testRunner.And("User swipe from \'left\' to \'right\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 227
 testRunner.And("User navigate to \'Pin Site Care\' page from \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 228
 testRunner.Then("The \'set daily time\' should be \'6:20 AM\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Swiping left with data entered in User Profile not lost")]
        public virtual void SwipingLeftWithDataEnteredInUserProfileNotLost()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Swiping left with data entered in User Profile not lost", ((string[])(null)));
#line 230
this.ScenarioSetup(scenarioInfo);
#line 231
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 232
 testRunner.And("User navigate to \'User Profile\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 233
 testRunner.When("User enter \'nickname\' with \'myNick\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 234
 testRunner.And("User swipe from \'right\' to \'left\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 235
 testRunner.And("User swipe from \'left\' to \'right\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 236
 testRunner.Then("The \'nickname\' should be \'myNick\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Successful navigation swapping from right to left")]
        public virtual void SuccessfulNavigationSwappingFromRightToLeft()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Successful navigation swapping from right to left", ((string[])(null)));
#line 238
this.ScenarioSetup(scenarioInfo);
#line 239
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 240
 testRunner.And("User navigate to \'Welcome User Anonymous\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 241
 testRunner.When("User swipe from \'right\' to \'left\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 242
 testRunner.And("User swipe from \'right\' to \'left\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 243
 testRunner.Then("\'Pin Site Care\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Successful navigation swapping from left to right")]
        public virtual void SuccessfulNavigationSwappingFromLeftToRight()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Successful navigation swapping from left to right", ((string[])(null)));
#line 245
this.ScenarioSetup(scenarioInfo);
#line 246
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 247
 testRunner.And("User navigate to \'Pin Site Care\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 248
 testRunner.When("User swipe from \'left\' to \'right\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 249
 testRunner.And("User swipe from \'left\' to \'right\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 250
 testRunner.Then("\'Welcome User Anonymous\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
