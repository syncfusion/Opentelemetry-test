using Syncfusion.Telemetry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OpenTelemetry
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // AWS CloudWatch
			var result = CustomerInsightsManager.SetupTelemetry(CustomerInsightPlaform.AwsCloudWatch,"AccessKeyId=<AWS_ACCESS_KEY_ID>;SecretAccessKey=<AWS_SECRET_ACCESS_KEY>;Region=<AWS_REGION>;LogGroupName=<CLOUDWATCH_LOG_GROUP_NAME>");

			// Azure Application Insights
			var result = CustomerInsightsManager.SetupTelemetry(CustomerInsightPlaform.Azure,"InstrumentationKey=<APPLICATION_INSIGHTS_INSTRUMENTATION_KEY>;IngestionEndpoint=<APPLICATION_INSIGHTS_INGESTION_ENDPOINT>;LiveEndpoint=<APPLICATION_INSIGHTS_LIVE_ENDPOINT>");
			if (!result.IsValid)
			{
				var message = result.Message;
			}
			else
			{
				var message = result.Message;    
			}
			Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_LICENSE_KEY");

        }
    }
}
