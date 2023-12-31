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
    [NUnit.Framework.DescriptionAttribute("MyContacts_item_4948")]
    public partial class MyContacts_Item_4948Feature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "MyContacts_item_4948.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "MyContacts_item_4948", "After skipping the first profile set-up\nI want to add/change my profile set-up in" +
                    "dividually", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Anonymous User access to User details page without first setup")]
        public virtual void AnonymousUserAccessToUserDetailsPageWithoutFirstSetup()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Anonymous User access to User details page without first setup", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.When("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then("\'User Details\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 11
 testRunner.And("The \'nickname\' should be \'empty\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Anonymous User add his nickname")]
        public virtual void AnonymousUserAddHisNickname()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Anonymous User add his nickname", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 14
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
 testRunner.And("User navigate to \'User Details\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
 testRunner.When("User enter \'nickname\' with \'-myNickname-\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
 testRunner.Then("The \'nickname\' should be \'-myNickname-\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Max Anonymous nickname lenght without first setup")]
        public virtual void MaxAnonymousNicknameLenghtWithoutFirstSetup()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Max Anonymous nickname lenght without first setup", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 21
 testRunner.And("User navigate to \'User Details\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.When("User enter \'nickname\' with \'12345678901234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.And("User tap on \'a char\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.Then("The \'nickname\' should be \'12345678901234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Not enabled chars without first setup anonymous")]
        public virtual void NotEnabledCharsWithoutFirstSetupAnonymous()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Not enabled chars without first setup anonymous", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line 27
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 28
 testRunner.And("User navigate to \'User Details\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
 testRunner.When("User enter \'nickname\' with \'ciao<>%#&?\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 30
 testRunner.Then("The \'nickname\' should be \'ciao\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Anonymous User access to User details page with first setup")]
        public virtual void AnonymousUserAccessToUserDetailsPageWithFirstSetup()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Anonymous User access to User details page with first setup", ((string[])(null)));
#line 34
this.ScenarioSetup(scenarioInfo);
#line 35
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table1.AddRow(new string[] {
                        "_Nickname",
                        "empty"});
#line 36
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table1, "When ");
#line 39
 testRunner.And("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
 testRunner.Then("\'User Details\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 41
 testRunner.And("The \'nickname\' should be \'_Nickname\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Anonymous User update his nickname")]
        public virtual void AnonymousUserUpdateHisNickname()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Anonymous User update his nickname", ((string[])(null)));
#line 43
this.ScenarioSetup(scenarioInfo);
#line 44
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table2.AddRow(new string[] {
                        "first_Nickname",
                        "empty"});
#line 45
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table2, "When ");
#line 48
 testRunner.And("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
 testRunner.And("User enter \'nickname\' with \'second_Nickname\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
 testRunner.Then("The \'nickname\' should be \'second_Nickname\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Max Anonymous nickname lenght with first setup")]
        public virtual void MaxAnonymousNicknameLenghtWithFirstSetup()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Max Anonymous nickname lenght with first setup", ((string[])(null)));
#line 52
this.ScenarioSetup(scenarioInfo);
#line 53
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table3.AddRow(new string[] {
                        "12345678901234567890",
                        "empty"});
#line 54
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table3, "When ");
#line 57
 testRunner.And("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.And("User tap on \'a char\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.Then("The \'nickname\' should be \'12345678901234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Not enabled chars with first setup anonymous")]
        public virtual void NotEnabledCharsWithFirstSetupAnonymous()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Not enabled chars with first setup anonymous", ((string[])(null)));
#line 61
this.ScenarioSetup(scenarioInfo);
#line 62
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table4.AddRow(new string[] {
                        "justAname",
                        "empty"});
#line 63
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table4, "When ");
#line 66
 testRunner.And("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
 testRunner.And("User enter \'nickname\' with \'ciao<>%#&?\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
 testRunner.Then("The \'nickname\' should be \'ciao\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Normal User access to User details page without first setup")]
        public virtual void NormalUserAccessToUserDetailsPageWithoutFirstSetup()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Normal User access to User details page without first setup", ((string[])(null)));
#line 72
this.ScenarioSetup(scenarioInfo);
#line 73
 testRunner.Given("User is \'normal\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 74
 testRunner.And("User navigate to \'Home Normal\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.When("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 76
 testRunner.Then("\'User Details\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 77
 testRunner.And("The \'nickname\' should be \'empty\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Normal User add his nickname")]
        public virtual void NormalUserAddHisNickname()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Normal User add his nickname", ((string[])(null)));
#line 79
this.ScenarioSetup(scenarioInfo);
#line 80
 testRunner.Given("User is \'normal\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 81
 testRunner.And("User navigate to \'User Details\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 82
 testRunner.When("User enter \'nickname\' with \'-myNickname-\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 83
 testRunner.Then("The \'nickname\' should be \'-myNickname-\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Max Normal nickname lenght without first setup")]
        public virtual void MaxNormalNicknameLenghtWithoutFirstSetup()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Max Normal nickname lenght without first setup", ((string[])(null)));
#line 85
this.ScenarioSetup(scenarioInfo);
#line 86
 testRunner.Given("User is \'normal\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 87
 testRunner.And("User navigate to \'User Details\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
 testRunner.When("User enter \'nickname\' with \'12345678901234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 89
 testRunner.And("User tap on \'a char\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
 testRunner.Then("The \'nickname\' should be \'12345678901234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Not enabled chars without first setup normal")]
        public virtual void NotEnabledCharsWithoutFirstSetupNormal()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Not enabled chars without first setup normal", ((string[])(null)));
#line 92
this.ScenarioSetup(scenarioInfo);
#line 93
 testRunner.Given("User is \'normal\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 94
 testRunner.And("User navigate to \'User Details\' page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
 testRunner.When("User enter \'nickname\' with \'ciao<>%#&?\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 96
 testRunner.Then("The \'nickname\' should be \'ciao\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Normal User access to User details page with first setup")]
        public virtual void NormalUserAccessToUserDetailsPageWithFirstSetup()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Normal User access to User details page with first setup", ((string[])(null)));
#line 100
this.ScenarioSetup(scenarioInfo);
#line 101
 testRunner.Given("User is \'normal\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "timePinSite",
                        "mytext",
                        "timeInsigth",
                        "GoalStat",
                        "InsightsStat"});
            table5.AddRow(new string[] {
                        "myNick",
                        "empty",
                        "this is my text",
                        "empty",
                        "enabled",
                        "enabled"});
#line 102
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table5, "When ");
#line 105
 testRunner.And("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
 testRunner.Then("\'User Details\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 107
 testRunner.And("The \'nickname\' should be \'myNick\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Normal User update his nickname")]
        public virtual void NormalUserUpdateHisNickname()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Normal User update his nickname", ((string[])(null)));
#line 109
this.ScenarioSetup(scenarioInfo);
#line 110
 testRunner.Given("User is \'normal\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "timePinSite",
                        "mytext",
                        "timeInsigth",
                        "GoalStat",
                        "InsightsStat"});
            table6.AddRow(new string[] {
                        "myNick",
                        "empty",
                        "this is my text",
                        "empty",
                        "enabled",
                        "enabled"});
#line 111
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table6, "When ");
#line 114
 testRunner.And("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
 testRunner.And("User enter \'nickname\' with \'second_Nickname\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
 testRunner.Then("The \'nickname\' should be \'second_Nickname\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Max Normal nickname lenght with first setup")]
        public virtual void MaxNormalNicknameLenghtWithFirstSetup()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Max Normal nickname lenght with first setup", ((string[])(null)));
#line 118
this.ScenarioSetup(scenarioInfo);
#line 119
 testRunner.Given("User is \'normal\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "timePinSite",
                        "mytext",
                        "timeInsigth",
                        "GoalStat",
                        "InsightsStat"});
            table7.AddRow(new string[] {
                        "12345678901234567890",
                        "empty",
                        "this is my text",
                        "empty",
                        "enabled",
                        "enabled"});
#line 120
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table7, "When ");
#line 123
 testRunner.And("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
 testRunner.And("User tap on \'a char\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
 testRunner.Then("The \'nickname\' should be \'12345678901234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Not enabled chars with first setup normal")]
        public virtual void NotEnabledCharsWithFirstSetupNormal()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Not enabled chars with first setup normal", ((string[])(null)));
#line 127
this.ScenarioSetup(scenarioInfo);
#line 128
 testRunner.Given("User is \'normal\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "timePinSite",
                        "mytext",
                        "timeInsigth",
                        "GoalStat",
                        "InsightsStat"});
            table8.AddRow(new string[] {
                        "myNick",
                        "empty",
                        "this is my text",
                        "empty",
                        "enabled",
                        "enabled"});
#line 129
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table8, "When ");
#line 132
 testRunner.And("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
 testRunner.And("User enter \'nickname\' with \'ciao<>%#&?\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
 testRunner.Then("The \'nickname\' should be \'ciao\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("When user set-up with anonymous and became normal then nickname is not lost")]
        public virtual void WhenUserSet_UpWithAnonymousAndBecameNormalThenNicknameIsNotLost()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("When user set-up with anonymous and became normal then nickname is not lost", ((string[])(null)));
#line 138
this.ScenarioSetup(scenarioInfo);
#line 139
 testRunner.Given("User is \'anonymous\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "nickname",
                        "time"});
            table9.AddRow(new string[] {
                        "_Nickname",
                        "empty"});
#line 140
 testRunner.When("User \'set and save\' profile with", ((string)(null)), table9, "When ");
#line 143
 testRunner.And("User became normal", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
 testRunner.When("User tap on \'Profile\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 145
 testRunner.Then("\'User Details\' page is visualized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 146
 testRunner.And("The \'nickname\' should be \'_Nickname\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
