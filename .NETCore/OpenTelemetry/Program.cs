using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Telemetry;

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
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_License_Key");
//Creates a new PDF document.
PdfDocument document = new PdfDocument();
//Creates a new page .
PdfPage page = document.Pages.Add();
//Creates a rectangle.
RectangleF rectangle = new RectangleF(10, 40, 30, 30);

//Creates a new popup annotation.
PdfPopupAnnotation popupAnnotation = new PdfPopupAnnotation(rectangle, "Test popup annotation");
popupAnnotation.Border.Width = 4;
popupAnnotation.Border.HorizontalRadius = 20;
popupAnnotation.Border.VerticalRadius = 30;
//Sets the pdf popup icon.
popupAnnotation.Icon = PdfPopupIcon.NewParagraph;
//Adds this annotation to the created page.
page.Annotations.Add(popupAnnotation);

//Save the document
document.Save("Output.pdf");
//Closes the document.
document.Close(true);