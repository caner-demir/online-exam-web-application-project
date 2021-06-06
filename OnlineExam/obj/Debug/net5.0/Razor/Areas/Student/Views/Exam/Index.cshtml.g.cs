#pragma checksum "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f612d153d6b8d7606f483b25252aeed9d31ac0ae"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Student_Views_Exam_Index), @"mvc.1.0.view", @"/Areas/Student/Views/Exam/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\_ViewImports.cshtml"
using OnlineExam;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\_ViewImports.cshtml"
using OnlineExam.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f612d153d6b8d7606f483b25252aeed9d31ac0ae", @"/Areas/Student/Views/Exam/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6c26b38a268148b41f8d27cf9435d899b998ac06", @"/Areas/Student/Views/_ViewImports.cshtml")]
    public class Areas_Student_Views_Exam_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<OnlineExam.Models.ViewModels.ExamsVM>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-dark font-weight-bold"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-area", "Student", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Exam", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-dark"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-fragment", "course-details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/student-exam-index.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<div class=""container-student-exam"">
    <div class=""container border-left border-right"">
        <div class=""row pt-4"" style=""background-color:#fafafa; min-height:85vh"">
            <div class=""col-9"">
                <div class=""mb-4 p-0 shadow-sm"" id=""course-details"">
                    <div class=""card rounded bg-primary"">
                        <div class=""card-header text-light"">
                            <h3 style=""margin:0px"">
                                <i class=""fas fa-info-circle text-light""></i>&nbsp; About this course
                            </h3>
                        </div>
                        <div class=""card-body bg-white rounded-bottom"">
                            <h2 class=""text-secondary"">");
#nullable restore
#line 15 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                  Write(Model.Course.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n                            ");
#nullable restore
#line 16 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                       Write(Html.Raw(Model.Course.DescriptionDetailed));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            <div class=\"text-secondary\">\r\n                                <div class=\"mb-2\" style=\"font-size:medium\"><i class=\"fas fa-user text-dark\"></i>&nbsp; Created by ");
#nullable restore
#line 18 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                                                                                             Write(Model.Course.ApplicationUser.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                                <div style=\"font-size:medium\"><i class=\"fas fa-users text-dark\"></i>&nbsp; ");
#nullable restore
#line 19 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                                                                      Write(Model.CountCourseUsers);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Students</div>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n\r\n");
#nullable restore
#line 25 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                 foreach (var exam in Model.Exams)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"mb-5\">\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "f612d153d6b8d7606f483b25252aeed9d31ac0ae8936", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 28 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => exam.StartDate);

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "id", 3, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1579, "exam-", 1579, 5, true);
#nullable restore
#line 28 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
AddHtmlAttributeValue("", 1584, exam.Id, 1584, 8, false);

#line default
#line hidden
#nullable disable
            AddHtmlAttributeValue("", 1592, "-startDate", 1592, 10, true);
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "f612d153d6b8d7606f483b25252aeed9d31ac0ae11210", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 29 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => exam.EndDate);

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "id", 3, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1681, "exam-", 1681, 5, true);
#nullable restore
#line 29 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
AddHtmlAttributeValue("", 1686, exam.Id, 1686, 8, false);

#line default
#line hidden
#nullable disable
            AddHtmlAttributeValue("", 1694, "-endDate", 1694, 8, true);
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                        <div class=\"col-12 mb-3 bd-callout bd-callout-danger bg-primary shadow-sm\"");
            BeginWriteAttribute("id", " id=\"", 1806, "\"", 1824, 2);
            WriteAttributeValue("", 1811, "exam-", 1811, 5, true);
#nullable restore
#line 30 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
WriteAttributeValue("", 1816, exam.Id, 1816, 8, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                            <span class=\"text-light\" style=\"font-size:larger\">\r\n                                <i class=\"fas fa-user-edit text-light\"></i>&nbsp; ");
#nullable restore
#line 32 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                                             Write(exam.DateCreated.ToString("MMMM dd yyyy HH:mm"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                            </span>
                        </div>
                        <div class=""card shadow-sm rounded"" style=""background-color:#828b93"">
                            <div class=""card-header p-2"">
                                <h3 class=""text-center text-light mb-0"">");
#nullable restore
#line 37 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                                   Write(exam.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n                            </div>\r\n                            <div class=\"card-body bg-white rounded-bottom\"");
            BeginWriteAttribute("style", " style=\"", 2462, "\"", 2470, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n                                <p class=\"card-text\">");
#nullable restore
#line 40 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                Write(Html.Raw(exam.Details));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                                <p><span class=\"font-weight-bold text-secondary\">Start Date:</span>&nbsp;");
#nullable restore
#line 41 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                                                                    Write(exam.StartDate.ToString("MMMM dd yyyy HH:mm"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                                <p><span class=\"font-weight-bold text-secondary\">Duration:</span>&nbsp;");
#nullable restore
#line 42 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                                                                  Write(exam.Duration.Hours);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Hours, ");
#nullable restore
#line 42 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                                                                                              Write(exam.Duration.Minutes);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Minutes</p>\r\n                                <div class=\"text-center\">\r\n                                    <button");
            BeginWriteAttribute("onclick", " onclick=\"", 2982, "\"", 3013, 3);
            WriteAttributeValue("", 2992, "beginExam(\'", 2992, 11, true);
#nullable restore
#line 44 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
WriteAttributeValue("", 3003, exam.Id, 3003, 8, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3011, "\')", 3011, 2, true);
            EndWriteAttribute();
            WriteLiteral(@" class=""btn btn-warning rounded-0 text-dark border border-secondary pt-2 pb-2"" style=""width:130px"">Begin Exam</button>
                                </div>
                            </div>
                        </div>
                    </div>
");
#nullable restore
#line 49 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"            </div>
            <div class=""col-3"">
                <div class=""card bg-primary shadow-sm mb-4""  style=""background:none"">
                    <div class=""card-header border-bottom"">
                        <h3 class=""text-center text-light mb-0""><i class=""fas fa-sitemap""></i>&nbsp; Navigation</h3>
                    </div>
                    <div class=""card-body bg-white rounded-bottom p-3 pl-4"">
");
#nullable restore
#line 57 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                          
                            foreach (var course in Model.CoursesNavigation)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <div class=\"mb-1\">\r\n                                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f612d153d6b8d7606f483b25252aeed9d31ac0ae18941", async() => {
                WriteLiteral("<i class=\"fas fa-shapes text-secondary\"></i>&nbsp; ");
#nullable restore
#line 62 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                                                                               Write(course.Name);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 62 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                         WriteLiteral(course.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                                </div>\r\n");
#nullable restore
#line 64 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                var exams = Model.ExamsNavigation.Where(e => e.CourseId == course.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <div class=\"pl-4 mb-1\">\r\n                                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f612d153d6b8d7606f483b25252aeed9d31ac0ae22380", async() => {
                WriteLiteral("<i class=\"fas fa-chevron-right text-secondary\"></i>&nbsp; About this course");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 67 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                          WriteLiteral(course.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Fragment = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                                </div>\r\n");
#nullable restore
#line 69 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                foreach (var exam in exams)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <div class=\"pl-4 mb-1\">\r\n                                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f612d153d6b8d7606f483b25252aeed9d31ac0ae25748", async() => {
                WriteLiteral("<i class=\"fas fa-chevron-right text-secondary\"></i>&nbsp; ");
#nullable restore
#line 73 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                                                                                                                       Write(exam.Name);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 73 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                             WriteLiteral(course.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
            WriteLiteral("exam-");
#nullable restore
#line 73 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                                                            WriteLiteral(exam.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Fragment = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-fragment", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Fragment, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                                    </div>\r\n");
#nullable restore
#line 75 "C:\Users\Caner Demir\Caner\OnlineExam\OnlineExam\Areas\Student\Views\Exam\Index.cshtml"
                                }
                            }
                        

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f612d153d6b8d7606f483b25252aeed9d31ac0ae30112", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    <script>\r\n        document.getElementById(\"container-main\").className = \"container-fluid p-0\";\r\n    </script>\r\n");
            }
            );
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<OnlineExam.Models.ViewModels.ExamsVM> Html { get; private set; }
    }
}
#pragma warning restore 1591
