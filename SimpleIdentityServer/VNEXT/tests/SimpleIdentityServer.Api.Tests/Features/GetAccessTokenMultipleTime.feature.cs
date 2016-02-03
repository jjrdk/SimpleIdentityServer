// ------------------------------------------------------------------------------
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
namespace SpecFlow.GeneratedTests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class GetAccessTokenMultipleTimeFeature : Xunit.IClassFixture<GetAccessTokenMultipleTimeFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "GetAccessTokenMultipleTime.feature"
#line hidden
        
        public GetAccessTokenMultipleTimeFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "GetAccessTokenMultipleTime", "As an authenticated user\nI request several times an access token", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
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
        
        public virtual void SetFixture(GetAccessTokenMultipleTimeFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "GetAccessTokenMultipleTime")]
        [Xunit.TraitAttribute("Description", "Request 3 times an access token")]
        public virtual void Request3TimesAnAccessToken()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Request 3 times an access token", ((string[])(null)));
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
 testRunner.Given("a resource owner with username thierry and password loki is defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.And("a mobile application MyHolidays is defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.And("allowed number of requests is 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.And("sliding time is 0.2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "client_id",
                        "username",
                        "password"});
            table1.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
            table1.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
            table1.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
#line 11
 testRunner.When("requesting access tokens", ((string)(null)), table1, "When ");
#line 17
 testRunner.Then("2 access tokens are generated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Message"});
            table2.AddRow(new string[] {
                        "Allow 2 requests per 0.2 minutes"});
#line 18
 testRunner.And("the errors should be returned", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "StatusCode",
                        "NumberOfRemainingRequests",
                        "NumberOfRequests"});
            table3.AddRow(new string[] {
                        "200",
                        "1",
                        "2"});
            table3.AddRow(new string[] {
                        "200",
                        "0",
                        "2"});
            table3.AddRow(new string[] {
                        "429",
                        "0",
                        "2"});
#line 22
 testRunner.And("the http responses should be returned", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "GetAccessTokenMultipleTime")]
        [Xunit.TraitAttribute("Description", "Request 5 times an access token")]
        public virtual void Request5TimesAnAccessToken()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Request 5 times an access token", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 29
 testRunner.Given("a resource owner with username thierry and password loki is defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
 testRunner.And("a mobile application MyHolidays is defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
 testRunner.And("allowed number of requests is 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
 testRunner.And("sliding time is 0.2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "client_id",
                        "username",
                        "password"});
            table4.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
            table4.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
            table4.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
            table4.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
            table4.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
#line 34
 testRunner.When("requesting access tokens", ((string)(null)), table4, "When ");
#line 42
 testRunner.Then("2 access tokens are generated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Message"});
            table5.AddRow(new string[] {
                        "Allow 2 requests per 0.2 minutes"});
            table5.AddRow(new string[] {
                        "Allow 2 requests per 0.2 minutes"});
            table5.AddRow(new string[] {
                        "Allow 2 requests per 0.2 minutes"});
#line 43
 testRunner.And("the errors should be returned", ((string)(null)), table5, "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "StatusCode",
                        "NumberOfRemainingRequests",
                        "NumberOfRequests"});
            table6.AddRow(new string[] {
                        "200",
                        "1",
                        "2"});
            table6.AddRow(new string[] {
                        "200",
                        "0",
                        "2"});
            table6.AddRow(new string[] {
                        "429",
                        "0",
                        "2"});
            table6.AddRow(new string[] {
                        "429",
                        "0",
                        "2"});
            table6.AddRow(new string[] {
                        "429",
                        "0",
                        "2"});
#line 49
 testRunner.And("the http responses should be returned", ((string)(null)), table6, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "GetAccessTokenMultipleTime")]
        [Xunit.TraitAttribute("Description", "Request 3 times an access token wait for 3 seconds and request 2 access tokens")]
        public virtual void Request3TimesAnAccessTokenWaitFor3SecondsAndRequest2AccessTokens()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Request 3 times an access token wait for 3 seconds and request 2 access tokens", ((string[])(null)));
#line 57
this.ScenarioSetup(scenarioInfo);
#line 58
 testRunner.Given("a resource owner with username thierry and password loki is defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 59
 testRunner.And("a mobile application MyHolidays is defined", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
 testRunner.And("allowed number of requests is 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
 testRunner.And("sliding time is 0.2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "client_id",
                        "username",
                        "password"});
            table7.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
            table7.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
            table7.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
#line 63
 testRunner.When("requesting access tokens", ((string)(null)), table7, "When ");
#line 68
 testRunner.And("waiting for 3000 seconds", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "client_id",
                        "username",
                        "password"});
            table8.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
            table8.AddRow(new string[] {
                        "MyHolidays",
                        "thierry",
                        "loki"});
#line 69
 testRunner.And("requesting access tokens", ((string)(null)), table8, "And ");
#line 74
 testRunner.Then("4 access tokens are generated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Message"});
            table9.AddRow(new string[] {
                        "Allow 2 requests per 0.2 minutes"});
#line 76
 testRunner.And("the errors should be returned", ((string)(null)), table9, "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "StatusCode",
                        "NumberOfRemainingRequests",
                        "NumberOfRequests"});
            table10.AddRow(new string[] {
                        "200",
                        "1",
                        "2"});
            table10.AddRow(new string[] {
                        "200",
                        "0",
                        "2"});
            table10.AddRow(new string[] {
                        "429",
                        "0",
                        "2"});
            table10.AddRow(new string[] {
                        "200",
                        "1",
                        "2"});
            table10.AddRow(new string[] {
                        "200",
                        "0",
                        "2"});
#line 80
 testRunner.And("the http responses should be returned", ((string)(null)), table10, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                GetAccessTokenMultipleTimeFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                GetAccessTokenMultipleTimeFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
