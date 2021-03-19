using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
namespace CV19.Infrastructure.Converters
{
    /// <summary> реализация линейного преобразования f(x) = k *x + b </summary>
    //можно указать из какого в какой идет конвертация и дизайнер будет понимать что является результатом метода Convert и ConvertBack
    [ValueConversion(typeof(double), typeof(double))]
    [MarkupExtensionReturnType(typeof(Linear))] //тогда в разметке будут видн свойства, хотя они и так сейчас видны
    internal class Linear : Converter
    {

        //добоаляем своцство коэфициент к и в
        //указываем что это свойсвто связано с элементом конструктора К, B
        [ConstructorArgument("K")]
        public double K { get; set; } = 1;
        [ConstructorArgument("B")]
        public double B { get; set; }  

        //конструкторы
        public Linear() {}
    
        public Linear(double K) => this.K = K;
        public Linear(double K, double B) : this(K) => this.B = B;

        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is null) return null;
            var x = System.Convert.ToDouble(v, c);
            return K * x + B;
        }
        public override object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            if (v is null) return null;
            var y = System.Convert.ToDouble(v, c);
            return (y - B) / K;
        }
    }
}
