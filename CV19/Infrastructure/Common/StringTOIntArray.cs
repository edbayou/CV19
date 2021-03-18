using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
namespace CV19.Infrastructure.Common
{
    //так как у нас возвраается обджект мы можем указать дизайнеру какой тип по факту возвращается через атрибут
    [MarkupExtensionReturnType(typeof(int[]))]
    internal class StringTOIntArray : MarkupExtension
    { 
        //чтобы была возможость в разметке сразу в класс передать значение в Str реализуем MarkupExtension
      

        public override object ProvideValue(IServiceProvider sp) => Str.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries).DefaultIfEmpty().Select(int.Parse).ToArray();
        [ConstructorArgument("Str")]
        public string Str { get; set; }
        public char Separator { get; set; } = ';';
        public StringTOIntArray() { }
        public StringTOIntArray(String Str) => this.Str = Str;


    }
}
