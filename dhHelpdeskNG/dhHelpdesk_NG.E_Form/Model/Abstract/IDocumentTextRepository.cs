using System;
using System.Collections.Generic;
using ECT.Model.Entities;

namespace ECT.Model.Abstract
{

    public interface IDocumentTextRepository
    {
        //Default Text
        string GetText(string value1, int? customerId);

        //Default Text with replace
        string GetText(string value1, string replaceValue1, int? customerId);

        //Text with texttype and replacevalue
        string GetText(string textType, string value1, string replaceValue1, int? customerId);


        string GetText(string textType, string value1, string operator1, string value2, string operator2);

        string GetText(string textType, string value1, string operator1, string value2, string operator2, string replaceValue1);

        string GetText(string textType, string value1, string operator1, string value2, string operator2, string replaceValue1, int? customerId);

        string GetText(string textType, string value1, string operator1, string value2, string operator2, string replaceValue1, int? customerId, Guid? formGuid);
    }

}