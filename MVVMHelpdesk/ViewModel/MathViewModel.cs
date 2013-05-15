using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel
{
    public class MathViewModel: ViewModelBase
    {
        private DateTime _dateBegin = new DateTime(2010,1,1);
        public DateTime DateBegin
        {
            get { return _dateBegin; }
            set {
                if (_dateBegin == value) return;
                _dateBegin = value;
                OnPropertyChanged(() => DateBegin);
            }
        }

        private DateTime _dateEnd = new DateTime(2010, 12, 31);
        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set
            {
                if (value == _dateEnd) return;
                _dateEnd = value;
                OnPropertyChanged(() => DateEnd);
            }
        }

        private bool? _isAdective;
        public bool? IsAdective
        {
            get { return _isAdective; }
            set
            {
                if (value != _isAdective)
                {
                    _isAdective = value;
                    OnPropertyChanged(() => IsAdective);
                }
            }

        }

        public double MiddleY { get; private set; }
        public double MiddleA { get; private set; }
        public double CorIndex { get; private set; }
        public double DetIndex { get; private set; }

        public double FisherCalc { get; private set; }
        public double FisherTable { get; private set; }

        public string Regression { get; private set; }

        private ICommand _solveCommand;
        public ICommand SolveCommand
        {
            get
            {
                _solveCommand = _solveCommand ?? new RelayCommand(_solve);
                return _solveCommand;
            }
        }

        private double[] _getX()
        {
            DateTime db = new DateTime(DateBegin.Year, DateBegin.Month, 1);
            DateTime de = new DateTime(DateEnd.Year, DateEnd.Month, DateTime.DaysInMonth(DateEnd.Year, DateEnd.Month));
            var monthCount = de.Year*12 + de.Month - db.Year*12 - db.Month + 1;
            var res = new double[monthCount];
            for (int i = 1; i <= monthCount; i++)
                res[i - 1] = i;
            return res;
        }

        private double[] _getY()
        {
            DateTime db = new DateTime(DateBegin.Year, DateBegin.Month, 1);
            DateTime de = new DateTime(DateEnd.Year, DateEnd.Month, DateTime.DaysInMonth(DateEnd.Year, DateEnd.Month));
            var monthCount = de.Year * 12 + de.Month - db.Year * 12 - db.Month + 1;
            var res = new double[monthCount];
            using (var context = new HelpdeskContext())
            {
                for (int i = 0; i < monthCount; i++)
                {
                    var d1 = db.AddMonths(i);
                    var d2 = db.AddMonths(i + 1).AddDays(-1);
                    res[i] = context.CartridgeAccountings.Count(w => w.StartDate >= d1 && w.StartDate <= d2);
                }
            }
            return res;
        }

        double _determinant(double[][] matrix)
        {
            var len = matrix.GetLength(0);

            if (len == 1)
            {
                return matrix[0][0];
            }

            if (len == 2)
            {
                return (matrix[0][0]*matrix[1][1]) - (matrix[0][1]*matrix[1][0]);
            }

            if (len==3)
            {
                return ((matrix[0][0]*matrix[1][1]*matrix[2][2]
                         + matrix[0][1]*matrix[1][2]*matrix[2][0]
                         + matrix[0][2]*matrix[1][0]*matrix[2][1])
                        - (matrix[0][0]*matrix[1][2]*matrix[2][1]
                           + matrix[0][1]*matrix[1][0]*matrix[2][2]
                           + matrix[0][2]*matrix[1][1]*matrix[2][0]));
            }

            return 0;
        }

        private void _solve(object canvas)
        {
            var numX = _getX();
            var numY = _getY();
            var nn = numX.Count();
            var numX2 = new double[nn];
            var numX3 = new double[nn];
            var numX4 = new double[nn];
            var numXY = new double[nn];
            var numX2Y = new double[nn];
            var eps = 4;
            var alpha = 0.05;

            for (var i = 0; i < nn; i++)
            {
                numX2[i] = numX[i]*numX[i];
                numX3[i] = numX[i]*numX[i]*numX[i];
                numX4[i] = numX[i]*numX[i]*numX[i]*numX[i];
                numXY[i] = numX[i]*numY[i];
                numX2Y[i] = numX[i]*numX[i]*numY[i];
            }

            var sumX = numX.Sum();
            var sumY = numY.Sum();
            var sumX2 = numX2.Sum();
            var sumX3 = numX3.Sum();
            var sumX4 = numX4.Sum();
            var sumXY = numXY.Sum();
            var sumX2Y = numX2Y.Sum();

            var SumX = Math.Round(sumX, eps);
            var SumY = Math.Round(sumY, eps);
            var SumX2 = Math.Round(sumX2, eps);
            var SumX3 = Math.Round(sumX3, eps);
            var SumX4 = Math.Round(sumX4, eps);
            var SumXY = Math.Round(sumXY, eps);
            var SumX2Y = Math.Round(sumX2Y, eps);

            var delta =
                _determinant(new[]
                    {
                        new[] {sumX2, sumX, nn}, 
                        new[] {sumX3, sumX2, sumX}, 
                        new[] {sumX4, sumX3, sumX2}
                    });
            var deltaA =
                _determinant(new[]
                    {
                        new[] {sumY, sumX, nn}, 
                        new[] {sumXY, sumX2, sumX}, 
                        new[] {sumX2Y, sumX3, sumX2}
                    });
            var deltaB = _determinant(new[]
                {
                    new[] {sumX2, sumY, nn},
                    new[] {sumX3, sumXY, sumX},
                    new[] {sumX4, sumX2Y, sumX2}
                });
            var deltaC = _determinant(new[]
                {
                    new[] {sumX2, sumX, sumY},
                    new[] {sumX3, sumX2, sumXY},
                    new[] {sumX4, sumX3, sumX2Y}
                });
            var Delta = Math.Round(delta, eps);
            var DeltaA = Math.Round(deltaA, eps);
            var DeltaB = Math.Round(deltaB, eps);
            var DeltaC = Math.Round(deltaC, eps);
            var a1 = deltaA / delta;
            var A = Math.Round(a1, eps);
            var b1 = deltaB / delta;
            var B = Math.Round(b1, eps);
            var c1 = deltaC / delta;
            var C = Math.Round(c1, eps);

            Regression = String.Format("{0}X*X + {1}X + {2}", A, B, C);
            OnPropertyChanged(() => Regression);

            var numY1 = new double[nn];
            var numYy = new double[nn];
            var numYY1 = new double[nn];
            var numYy2 = new double[nn];
            var numYY12 = new double[nn];
            var numA = new double[nn];
            var numEE = new double[nn];
            var numEE2 = new double[nn];
            var NumEE2 = new double[nn];
            var srY = (numY.Sum() / nn);
            MiddleY = srY;
            OnPropertyChanged(() => MiddleY);

            for (var i = 0; i < nn; i++)
            {
                numY1[i] = ((deltaA / delta) * numX[i] * numX[i] + (deltaB / delta) * numX[i] + (deltaC / delta));
                numYy[i] = (numY[i] - srY);
                numYY1[i] = (numY[i] - numY1[i]);
                numYy2[i] = (numYy[i] * numYy[i]);
                numYY12[i] = (numYY1[i] * numYY1[i]);
                numA[i] = Math.Abs(numYY1[i] / numY[i]);
            }

            for (var j = 1; j < nn; j++)
            {
                numEE[j] = (numYY1[j] - numYY1[j - 1]);
                numEE2[j] = (numEE[j] * numEE[j]);
                NumEE2[j] = (numEE[j] * numEE[j]);
            }

            numEE2 = _shift(numEE2);

            var sumYy2 = numYy2.Sum();
            var SumYy2 = Math.Round(sumYy2, eps);
            var sumYY12 = numYY12.Sum();
            var SumYY12 = Math.Round(sumYY12, eps);
            var sumA = numA.Sum();
            var SumA = Math.Round(sumA, eps);
            var sumEE2 = numEE2.Sum();
            var SumEE2 = Math.Round(sumEE2, eps);
            var Ysr = Math.Round(sumY/nn, eps);
            var R = Math.Round(Math.Sqrt(1 - (sumYY12/sumYy2)), eps);
            CorIndex = R; OnPropertyChanged(() => CorIndex);
            var R2 = Math.Round((1 - (sumYY12/sumYy2)), eps);
            DetIndex = R2; OnPropertyChanged(() => DetIndex);
            var Ae = Math.Round((sumA/nn)*100, eps);
            MiddleA = Ae; OnPropertyChanged(() => MiddleA);
            var Ff = Math.Round(((1 - (sumYY12/sumYy2))/(sumYY12/sumYy2))*((nn - 3)/2), eps);
            FisherCalc = Ff; OnPropertyChanged(() => FisherCalc);
            var Ft = Math.Round(AFishF(alpha, 2, (nn - 3)), eps);
            FisherTable = Ft; OnPropertyChanged(() => FisherTable);
            var dF = Math.Round(sumEE2 / sumYY12, eps);
            IsAdective = Ff < Ft;

            var cnvs = (System.Windows.Controls.Canvas)canvas;
            double xMin = numX.Min();
            double xMax = numX.Max();
            double yMin = numY.Min();
            double yMax = numY.Max();

            double kX = (500 - 20)/(xMax - xMin);
            double kY = (300 - 20)/(yMax - yMin);

            var months = new string[] {"", "ЯНВ", "ФЕВ", "МАР","АПР", "МАЙ", "ИЮН", "ИЮЛ","АВГ","СЕН","ОКТ","НОЯ","ДЕК"};
            var db = new DateTime(DateBegin.Year, DateBegin.Month, 1);

            cnvs.Children.Clear();
            for (int i = 0; i < nn; i++)
            {
                {
                    var p = new System.Windows.Shapes.Ellipse {Fill = System.Windows.Media.Brushes.Navy};
                    var x = (int) ((numX[i] - xMin)*kX + 10) - 2;
                    var y = (int) (300 - ((numY[i] - yMin)*kY + 10)) - 2;
                    System.Windows.Controls.Canvas.SetLeft(p, x);
                    System.Windows.Controls.Canvas.SetTop(p, y);
                    p.Width = 5;
                    p.Height = 5;
                    cnvs.Children.Add(p);
                    var t = new System.Windows.Controls.TextBlock { Text = numY[i].ToString() };
                    System.Windows.Controls.Canvas.SetLeft(t, x + 5);
                    System.Windows.Controls.Canvas.SetTop(t, y - 2);
                    cnvs.Children.Add(t);

                    var m = new System.Windows.Controls.TextBlock { Text = months[db.Month] };
                    System.Windows.Controls.Canvas.SetLeft(m, x + 5);
                    System.Windows.Controls.Canvas.SetTop(m, 300);
                    cnvs.Children.Add(m);
                    db = db.AddMonths(1);
                }
            }

            double xx = xMin;
            while (xx < xMax)
            {
                var p = new System.Windows.Shapes.Line();
                p.Stroke = System.Windows.Media.Brushes.Red;
                p.StrokeThickness = 2;
                {
                    var cx = xx;
                    var x = (int) ((cx - xMin)*kX + 10) - 1;
                    var cy = a1*cx*cx + b1*cx + c1;
                    var y = (int) (300 - ((cy - yMin)*kY + 10)) - 1;
                    p.X1 = x;
                    p.Y1 = y;
                }
                xx += 0.25;
                {
                    var cx = xx;
                    var x = (int)((cx - xMin) * kX + 10) - 1;
                    var cy = a1 * cx * cx + b1 * cx + c1;
                    var y = (int)(300 - ((cy - yMin) * kY + 10)) - 1;
                    p.X2 = x;
                    p.Y2 = y;
                }
                cnvs.Children.Add(p);
            }
        }

        private double AFishF(double p, int n1, int n2)
        {
            var v = 0.5;
            var dv = 0.5;
            double f = 0;
            while (dv > 1e-10)
            {
                f = 1 / v - 1;
                dv = dv/2;
                if (FishF(f, n1, n2) > p)
                {
                    v = v - dv;
                }
                else
                {
                    v = v + dv;
                }
            }
            return f;
        }

        private static double FishF(double f, int n1, int n2)
        {
            var x = n2/(n1*f + n2);
            if ((n1%2) == 0)
            {
                return (StatCom(1 - x, n2, n1 + n2 - 4, n2 - 2)*Math.Pow(x, n2/2));
            }
            if ((n2%2)==0)
            {
                return (1 - StatCom(x, n1, n1 + n2 - 4, n1 - 2)*Math.Pow(1 - x, n1/2));
            }
            var th = Math.Atan(Math.Sqrt(n1*f/n2));
            var a = th/(Math.PI/2);
            var sth = Math.Sin(th);
            var cth = Math.Cos(th);
            if (n2>1)
            {
                a = a + sth*cth*StatCom(cth*cth, 2, n2 - 3, -1)/(Math.PI/2);
            }
            if (n1==1)
            {
                return 1 - a;
            }
            var c = 4*StatCom(sth*sth, n2 + 1, n1 + n2 - 4, n2 - 2)*sth*Math.Pow(cth, n2)/Math.PI;
            if (n2==1)
            {
                return 1 - a + c/2;
            }
            var k=2;
            while (k <= (n2 - 1) / 2) 
            {
                c = c * k / (k - .5);
                k = k + 1;
            }
            return (1 - a + c);
        }

        private static double StatCom(double q, int i, int j, int b)
        {
            double zz=1;
            var z=zz;
            var k = i;
            while (k <= j)
            {
                zz = (zz*q*k)/(k - b);
                z = z + zz;
                k = k + 2;
            }
            return z;
        }

        private double[] _shift(double[] numEe2)
        {
            var shft = new double[numEe2.Count()];
            for (int i = 1; i < numEe2.Count(); i++)
            {
                shft[i - 1] = numEe2[i];
            }
            return shft;
        }
    }
}
