using System;
using System.Runtime.Serialization;

namespace physics
{

    public class ComplexDivideByZeroException : DivideByZeroException
    {
        public ComplexDivideByZeroException() {}

        public ComplexDivideByZeroException(string message) : base(message) {}

        public ComplexDivideByZeroException(string message, Exception innerException) : base(message, innerException) {}

        protected ComplexDivideByZeroException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    public partial class ComplexNumber
    {
        public double Re { get; }
        public double Im { get; }

        public ComplexNumber() 
            : this(0.0, 0.0) {}

        public ComplexNumber(double real, double im)
        {
            Re = real;
            Im = im;
        }

        public ComplexNumber(ComplexNumber other)
        {
            this.Re = other.Re;
            this.Im = other.Im;
        }

        /// <summary>
        /// Определяет, равен ли заданный объект текущему объекту.
        /// </summary>
        /// <returns>
        /// Значение true, если указанный объект равен текущему объекту; в противном случае — значение false.
        /// </returns>
        /// <param name="obj">Объект, который требуется сравнить с текущим объектом. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var complex = obj as ComplexNumber;
            if (complex != null)
                return Math.Abs(this.Re - complex.Re) < double.Epsilon &&
                       Math.Abs(this.Im - complex.Im) < double.Epsilon;

            return false;
        }

        public override string ToString() => 
            $@"({this.Re}, {this.Im})";
        

        public override int GetHashCode()
        {
            return (this.Re.GetHashCode() % 83924739)^ (this.Im.GetHashCode() + 2016 /* Просто я так хочу */);
        }

        public double Magnitude => Math.Sqrt(Sqr(Re) + Sqr(Im));

        public double Phase => Math.Atan2(Im, Re);
    }


    // В этой части только static методы
    public partial class ComplexNumber
    {
        public static double E = Math.E;
        public static double Pi = Math.PI;

        public static ComplexNumber Zero = new ComplexNumber(0.0, 0.0);
        public static ComplexNumber One = new ComplexNumber(1.0, 0.0);
        public static ComplexNumber ImaginaryOne = new ComplexNumber(0.0, 1.0);

        public static ComplexNumber NaN = new ComplexNumber(double.NaN, double.NaN);

        // Операторы преобразования
        public static implicit operator ComplexNumber(double d)
        {
            return new ComplexNumber(d, 0.0);
        }

        public static implicit operator ComplexNumber(int n)
        {
            return new ComplexNumber(n, 0.0);
        }

        public static explicit operator double(ComplexNumber c)
        {
            return c.Re;
        }

        public static explicit operator int(ComplexNumber c)
        {
            return (int) c.Re;
        }

        // Операторы + , - для комплексных
        public static ComplexNumber operator +(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber(left.Re + right.Re, left.Im + right.Im);
        }

        public static ComplexNumber operator -(ComplexNumber left, ComplexNumber right)
        {
            return new ComplexNumber(left.Re - right.Re, left.Im - right.Im);
        }

        public static ComplexNumber operator -(ComplexNumber c)
        {
            return new ComplexNumber(-c.Re, -c.Im);
        }

        // Тоже самое для целых

        public static ComplexNumber operator +(ComplexNumber left, int right)
        {
            return new ComplexNumber(left.Re + right, left.Im);
        }

        public static ComplexNumber operator +(int left, ComplexNumber right)
        {
            return new ComplexNumber(right.Re + left, right.Im);
        }

        public static ComplexNumber operator -(ComplexNumber left, int right)
        {
            return new ComplexNumber(left.Re - right, left.Im);
        }

        public static ComplexNumber operator -(int left, ComplexNumber right)
        {
            return new ComplexNumber(right.Re - left, right.Im);
        }

        //Тоже самое для вещественных

        public static ComplexNumber operator +(ComplexNumber left, double right)
        {
            return new ComplexNumber(left.Re + right, left.Im);
        }

        public static ComplexNumber operator +(double left, ComplexNumber right)
        {
            return new ComplexNumber(right.Re + left, right.Im);
        }

        public static ComplexNumber operator -(ComplexNumber left, double right)
        {
            return new ComplexNumber(left.Re - right, left.Im);
        }

        public static ComplexNumber operator -(double left, ComplexNumber right)
        {
            return new ComplexNumber(right.Re - left, right.Im);
        }

        // Операция *, / для комплексных

        public static ComplexNumber operator *(ComplexNumber left, ComplexNumber right)
        {
            double re = left.Re*right.Re - left.Im*right.Im;
            double im = left.Re*right.Im + left.Im*right.Re;

            return new ComplexNumber(re, im);
        }

        public static ComplexNumber operator /(ComplexNumber left, ComplexNumber right)
        {
            double div = right.Re*right.Re + right.Im*right.Im;

            if (Math.Abs(div) < double.Epsilon)
                throw new ComplexDivideByZeroException("Ошибка! Деление на ноль!");

            double re = (left.Re*right.Re + left.Im*right.Im)/div;
            double im = (left.Im*right.Re - left.Re*right.Im)/div;

            return new ComplexNumber(re, im);
        }

        public static ComplexNumber operator /(ComplexNumber left, double right)
        {
            if (Math.Abs(right) < double.Epsilon)
                throw new ComplexDivideByZeroException("Ошибка! Деление на ноль!");

            double re = left.Re/right;
            double im = left.Im/right;

            return new ComplexNumber(re, im);
        }

        public static ComplexNumber operator /(ComplexNumber left, int right)
        {
            if (right == 0)
                throw new ComplexDivideByZeroException("Ошибка! Деление на ноль!");

            double re = left.Re/right;
            double im = left.Im/right;

            return new ComplexNumber(re, im);
        }

        // Равенство и неравенство
        public static bool operator ==(ComplexNumber left, ComplexNumber right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ComplexNumber left, ComplexNumber right)
        {
            return !(left == right);
        }

        public static bool IsNaN(ComplexNumber c)
        {
            return double.IsNaN(c.Re) || double.IsNaN(c.Im);
        }

        // Разный комплексный матан

        public static double Abs(ComplexNumber c)
        {
            return c.Magnitude;
        }

        /// <summary>
        /// Возвращает логарифм с (основанием e) указанного числа.
        /// </summary>
        public static ComplexNumber Log(ComplexNumber c)
        {
            return new ComplexNumber(Math.Log(c.Re * c.Re + c.Im * c.Im)/2.0, c.Phase);
        }

        public static ComplexNumber Log(ComplexNumber c, double newBase)
        {
            var baseLog = Math.Log(newBase);

            return Log(c)/baseLog;
        }

        public static ComplexNumber Log(ComplexNumber c, ComplexNumber newBase)
        {
            return Log(c)/Log(newBase);
        }

        public static ComplexNumber Log10(ComplexNumber c)
        {
            return ComplexNumber.Log(c, 10);
        }

        public static ComplexNumber Exp(ComplexNumber c)
        {
            var exp = Math.Exp(c.Re);

            return new ComplexNumber(exp * Math.Cos(c.Im), exp * Math.Sin(c.Im));
        }

        public static ComplexNumber Pow(ComplexNumber c, int n)
        {
            var mag = Math.Pow(c.Magnitude, n);
            var ph = c.Phase*n;

            return new ComplexNumber(mag * Math.Cos(ph), mag * Math.Sin(ph));
        }

        public static ComplexNumber Pow(ComplexNumber c, ComplexNumber z)
        {
            return ComplexNumber.Exp(z * ComplexNumber.Log(c));
        }

        public static ComplexNumber Sqrt(ComplexNumber c)
        {
            return Pow(c, 0.5);
        }

        public static ComplexNumber Sqr(ComplexNumber c)
        {
            return Pow(c, 2);
        }

        private static double Sqr(double d) => d*d;

        public static ComplexNumber Conjugate(ComplexNumber c)
        {
            return new ComplexNumber(c.Re, -c.Im);
        }

        public static ComplexNumber Reciprocal(ComplexNumber c)
        {
            var div = Sqr(c.Re) + Sqr(c.Im);

            return new ComplexNumber(c.Re/div, -c.Im/div);
        }

        public static ComplexNumber Sin(ComplexNumber c)
        {
            var p = ComplexNumber.ImaginaryOne*c;

            var e1 = ComplexNumber.Exp(p);
            var e2 = ComplexNumber.Exp(-p);

            var i2 = new ComplexNumber(0.0, 2.0);

            return (e1 - e2)/i2;
        }

        public static ComplexNumber Cos(ComplexNumber c)
        {
            var p = ComplexNumber.ImaginaryOne * c;

            var e1 = ComplexNumber.Exp(p);
            var e2 = ComplexNumber.Exp(-p);

            return (e1 + e2)/2;
        }

        public static ComplexNumber Tan(ComplexNumber c)
        {
            return ComplexNumber.Sin(c)/ComplexNumber.Cos(c);
        }

        public static ComplexNumber Cot(ComplexNumber c)
        {
            return ComplexNumber.Cos(c)/ComplexNumber.Sin(c);
        }

        // обратные тригонометрические функции

        public static ComplexNumber Arcsin(ComplexNumber c)
        {
            return -ImaginaryOne * Log(ImaginaryOne * c + ComplexNumber.Sqrt(1 - ComplexNumber.Sqr(c)));
        }

        public static ComplexNumber Arccos(ComplexNumber c)
        {
            return -ImaginaryOne * Log(c + ComplexNumber.Sqrt(1 - ComplexNumber.Sqr(c)));
        }

        public static ComplexNumber Arctan(ComplexNumber c)
        {
            // тут конечно с точностью до k*Pi, но что поделать
            ComplexNumber ic = ImaginaryOne*c;
            return -ImaginaryOne/2 * Log((1 + ic)/(1 - ic));
        }

        public static ComplexNumber Arccot(ComplexNumber c)
        {
            ComplexNumber ic = ImaginaryOne * c;
            return -ImaginaryOne / 2 * Log((ic - 1) / (ic + 1));
        }

        // гиперболические

        /// <summary>
        /// Гиперболический синус
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ComplexNumber Sinh(ComplexNumber c)
        {
            return (Exp(c) - Exp(-c))/2;
        }

        /// <summary>
        /// Гиперболический косинус
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ComplexNumber Cosh(ComplexNumber c)
        {
            return (Exp(c) + Exp(-c)) / 2;
        }

        /// <summary>
        /// Гиперболический тангенс
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ComplexNumber Tanh(ComplexNumber c)
        {
            return Sinh(c)/Cosh(c);
        }

        /// <summary>
        /// Гиперболический котангенс
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ComplexNumber Coth(ComplexNumber c)
        {
            return Cosh(c)/Sinh(c);
        }


        // обратные гиперболические

    }
}