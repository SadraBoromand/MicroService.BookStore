#pragma checksum "D:\Larn\University Eduction\001\پروژه کارشناسی\Service.BookStore\src\Microservice.Seles\Views\Bascket\Payment.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "34fea27fa715c955a0263a73295acc60a2ce3a45"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Bascket_Payment), @"mvc.1.0.view", @"/Views/Bascket/Payment.cshtml")]
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
#line 1 "D:\Larn\University Eduction\001\پروژه کارشناسی\Service.BookStore\src\Microservice.Seles\Views\_ViewImports.cshtml"
using Microservice.Seles;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Larn\University Eduction\001\پروژه کارشناسی\Service.BookStore\src\Microservice.Seles\Views\_ViewImports.cshtml"
using Microservice.Seles.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"34fea27fa715c955a0263a73295acc60a2ce3a45", @"/Views/Bascket/Payment.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a7de7e85172bb9eae8ac5b79f60fe4a2fca8c4f2", @"/Views/_ViewImports.cshtml")]
    public class Views_Bascket_Payment : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<string>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "D:\Larn\University Eduction\001\پروژه کارشناسی\Service.BookStore\src\Microservice.Seles\Views\Bascket\Payment.cshtml"
  
    ViewData["Title"] = "Payment";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"text-center\">\r\n    <div class=\"col-md-8\">\r\n        <div class=\"alert alert-success p-5\">\r\n            <h1>پرداخت با موفقیت انجام شد</h1>\r\n            <hr />\r\n            <p>سریال فاکتور پرداخت : ");
#nullable restore
#line 11 "D:\Larn\University Eduction\001\پروژه کارشناسی\Service.BookStore\src\Microservice.Seles\Views\Bascket\Payment.cshtml"
                                Write(Model);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n            <br />\r\n            <a class=\"btn btn-success px-5\" href=\"/\">برگشت به سبد خرید</a>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<string> Html { get; private set; }
    }
}
#pragma warning restore 1591
