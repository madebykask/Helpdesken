using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECT.Service
{
    public class FormSeed
    {
        public Form Init(Guid guid)
        {
            var form = new Form();
            form.FormGuid = guid;
            form.Id = 1;
            form.FormVersion = Guid.NewGuid();
            form.CustomerId = 1;
            form.Status = Status.Working;
            form.Tabs = Tabs();

            return form;
        }

        public List<Form> Forms(Guid guid)
        {
            var forms = new List<Form>();

            var form = new Form();
            form.Id = 1;
            form.Name = "Absences";
            form.Description = "Registration of Absences Categories not registered in the Timekeeping System.";
            forms.Add(form);

            form = new Form();
            form.Id = 2;
            form.Name = "Hiring";
            form.Description = "Hiring a new co-worker, Re-hiring a co-worker returning to IKEA UK, IKEA Co-workers moving between countries.";
            forms.Add(form);

            form = new Form();
            form.Id = 3;
            form.Name = "Terminate Employment";
            form.Description = "Co-workers leaving IKEA.";
            forms.Add(form);

            form = new Form();
            form.Id = 3;
            form.Name = "Additional Payment or Payroll Deduction";
            form.Description = "Additonal Allowances to Basic Pay/Salary or Payroll Deductions.";
            forms.Add(form);

            form = new Form();
            form.Id = 4;
            form.Name = "Change Employment Terms & Conditions";
            form.Description = "Changes to Company, Business Unit, Function, Team, Job, Cost Centre or Basic Pay/Salary changes.";
            forms.Add(form);

            return forms;
        }

        private List<Element> Tab1Elements()
        {
            var elements = new List<Element>();

            var text = new TextBox();
            text.Id = 1;
            text.Label = "Co-Worker First Name";
            text.Name = "Co-Worker";
            text.Description = "Information text";
            text.SortOrder = 1;
            elements.Add(text);

            var paragraph = new Paragraph();
            paragraph.Id = 2;
            paragraph.Name = "Paragraph";
            paragraph.Label = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            elements.Add(paragraph);

            var date = new Date();
            date.Id = 3;
            date.Label = "Date";
            date.Name = "Date";
            date.Description = "Information text";
            date.SortOrder = 2;
            elements.Add(date);

            return elements;
        }

        private List<Element> Tab2Elements()
        {
            var elements = new List<Element>();

            var text = new TextBox();
            text.Id = 4;
            text.Multiline = true;
            text.Label = "Textarea";
            text.Name = "Description";
            text.Description = "Information text";
            text.SortOrder = 3;
            elements.Add(text);

            var dropdown = new Dropdown();
            dropdown.Id = 5;
            dropdown.Label = "Select";
            dropdown.Name = "Select";
            var options = new List<Option>();
            for(int i = 0; i < 10; i++)
            {
                options.Add(new Option { Id = i, Text = "Option " + i, Value = "Value " + i });
            }

            dropdown.Options = options;
            dropdown.SortOrder = 1;
            elements.Add(dropdown);

            return elements;
        }

        private List<Tab> Tabs()
        {
            var tabs = new List<Tab>();

            tabs.Add(new Tab { Id = 1, Name = "Service Request Details", SortOrder = 1, Elements = Tab1Elements() });
            tabs.Add(new Tab { Id = 2, Name = "Other", SortOrder = 2, Elements = Tab2Elements() });

            return tabs;
        }
    }
}
