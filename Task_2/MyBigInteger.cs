using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_2
{
    class MyBigInteger
    {
        private const int basisSize = 4;
        private int basis = (int)Math.Pow(10, basisSize);
        private sbyte signum { get; set; } // (-1 or 1 )
        private List<Int32> number {get; set;} 

        public MyBigInteger(sbyte signum, List<Int32> number)
        {
            this.signum = signum;
            this.number = number;
        }

        public MyBigInteger(string a)
        {
            if (a.StartsWith("-"))
            {
                this.signum = -1;
                a = a.Replace("-", "");
            }
            else
            {
                this.signum = 1;
            }

            a = RemoveStartZeros(a);
            number = new List<Int32>();
            while (a.Length > 0)
            { 
                int start_i = Math.Max(0, a.Length - basisSize);
                number.Add(Convert.ToInt32(a.Substring(start_i)));
                a = a.Remove(start_i);
            }

        }

        public int NumberSize ()
        {
            return this.WriteNumber().Length;        
        }

        public int PartSize()
        {
            return this.number.Count();
        }

        public string WriteNumber()
        {
            string output = "", temp;
            for (var i = number.Count - 1; i>=0; i--)
            {
                temp = number[i].ToString();
                if (i == number.Count-1)
                {
                    output += temp;
                }
                else
                {
                    output += (new string ('0', basisSize - temp.Length)) + temp; 
                }                        
            }
            output = RemoveStartZeros(output);
            return (signum == -1 ? "-" : "") + output;                
        }

        public string RemoveStartZeros(string text)
        {
            text = text.TrimStart('0');
            if (text == "")
            {
                return "0";
            }
            else
            {
                return text;
            }
        }
 
        public MyBigInteger NegativeNumber()
        {
            sbyte new_signum = (this.signum == 1 ? (sbyte)-1 : (sbyte)1);
            return new MyBigInteger(new_signum, this.number);
        }

        public MyBigInteger Add(MyBigInteger b)
        {
            sbyte new_signum = 1;
            
            if (b.signum != this.signum)
            {
                if (b.signum == -1)
                {
                    return this.Subtract(b.NegativeNumber());
                }
                else
                {
                    return b.Subtract(this.NegativeNumber());
                }
            }
            else
            {
                if(this.signum == -1)
                {
                    new_signum = -1;
                }
            }
            
            List<Int32> numbers = new List<Int32>();
            int max_length = Math.Max(this.PartSize(), b.PartSize());
            Int32 sum;
            Int32 temp_sum = 0;
            Int32 numb_1;
            Int32 numb_2;

            for(int i=0; i < max_length; i++)
            {
                if (i >= this.number.Count())
                {
                    numb_1 = 0;
                }
                else
                {
                    numb_1 = this.number[i];
                }

                if (i >= b.number.Count())
                {
                    numb_2 = 0;
                }
                else
                {
                    numb_2 = b.number[i];
                }

                sum = numb_1 + numb_2 + temp_sum;
                temp_sum = sum / basis;
                numbers.Add(sum % basis);
            }

            if (temp_sum > 0)
            {
                numbers.Add(temp_sum);
            }
            return new MyBigInteger(new_signum, numbers);
        }
        
        public MyBigInteger Subtract(MyBigInteger b)
        {
            if (b.signum != this.signum)
            {
                if (b.signum == -1)
                {
                    return this.Add(b.NegativeNumber());
                }
                else
                {
                    return (this.NegativeNumber().Add(b)).NegativeNumber();
                }
            }
            else
            {
                sbyte new_signum = 1;
                List<Int32> numbers = new List<Int32>();

                switch (this.Comparison(b) )
                {              
                    case '<':
                        {
                            new_signum = -1;
                            break;
                        }
                    case '=':
                        {
                            return new MyBigInteger("0");
                        }
                }

                Int32 temp_razn = 0;
                Int32 max_length = Math.Max(this.PartSize(), b.PartSize());
                Int32 numb_1;
                Int32 numb_2;
                Int32 razn;

                for (int i=0; i < max_length; i++)
                {
                    if (i >= this.number.Count())
                    {
                        numb_1 = 0;
                    }
                    else
                    {
                        numb_1 = this.number[i];
                    }

                    if (i >= b.number.Count())
                    {
                        numb_2 = 0;
                    }
                    else
                    {
                        numb_2 = b.number[i];
                    }

                    if(new_signum == -1 && this.signum == 1)
                    {
                        razn = basis + numb_2 - numb_1 - temp_razn; 
                    }
                    else
                    {
                        razn = basis + numb_1 - numb_2 - temp_razn;
                    }
                    
                    temp_razn = (razn / basis == 1 ? 0 : 1);
                    numbers.Add(razn % basis);
                }

                return new MyBigInteger(new_signum, numbers);
            }  
        }
        
        public char Comparison (MyBigInteger b)
        {
            if (this.signum != b.signum)
            {
                return (this.signum == -1 ? '<' : '>');
            }
            else
            {
                int this_size = this.NumberSize();
                int b_size = b.NumberSize();

                if (this_size != b_size)
                {
                    if (this_size < b_size)
                    {
                        return (this.signum == 1 ? '<' : '>');
                    }
                    else
                    {
                        return (this.signum == 1 ? '>' : '<');
                    }
                }
                else
                {
                    if (this.PartSize() == b.PartSize())
                    {
                        for (int i = this.PartSize() - 1; i >= 0; i--)
                        {
                            if (this.number[i] < b.number[i])
                            {
                                return (this.signum == 1 ? '<' : '>');
                            }
                            if (this.number[i] > b.number[i])
                            {
                                return (this.signum == 1 ? '>' : '<');
                            }
                        }
                        return '=';
                    }
                    else
                    {
                        return (this.PartSize() > b.PartSize() ? '>' : '<');
                    }
                    
                }
            }
        }

        public MyBigInteger Multiply (MyBigInteger b)
        {
            sbyte new_signum;
            List<Int32> numbers = new List<Int32>();

            if (this.IsZero() || b.IsZero())
            {
                return new MyBigInteger("0");
            }

            if (this.signum != b.signum)
            {
                new_signum = -1;
            }
            else
            {
                new_signum = 1;
            }

            ulong mult;
            Int32 k;

            for(int i=0; i < this.PartSize(); i++)
            {
                for (int j = 0; j < b.PartSize(); j++)
                {
                    mult = (ulong)this.number[i] * (ulong)b.number[j];

                    k = i + j;
                    while (k + 1 > numbers.Count())
                    {
                        numbers.Add(0);
                    }
                    numbers[k] += (int)(mult % (ulong)basis);

                    if(mult >= (ulong)basis)
                    {
                        if (numbers.Count() == k + 1)
                        {
                            numbers.Add(0);
                        }
                        numbers[k + 1] += (int)(mult / (ulong)basis);   
                    }
                }
            }

            for(int l = 0; l < numbers.Count(); l++)
            {
                if (numbers[l] >= basis)
                {
                    if (numbers.Count == l + 1)
                    {
                        numbers.Add(0);
                    }
                    numbers[l + 1] += numbers[l] / basis;
                    numbers[l] = numbers[l] % basis;
                }
            }
            
            return new MyBigInteger(new_signum, numbers);
        }

        public void DivRem(MyBigInteger b, out MyBigInteger answer, out MyBigInteger ostatok)
        {
            if (b.IsZero())
            {
                Console.WriteLine("Divide by zero !!!");
                answer = new MyBigInteger("0");
                ostatok = new MyBigInteger("0");
                return;
            }

            if (this.IsZero())
            {
                answer = new MyBigInteger("0");
                ostatok = new MyBigInteger("0");
                return;
            }

            Int32 down;
            Int32 up;
            Int32 temp_i = this.PartSize() - 1;
            MyBigInteger now_numb = new MyBigInteger(1, new List<Int32>());
            MyBigInteger temp_bigInt;
            List<Int32> numbers_answer = new List<Int32>();
            bool flag = false;
            bool flag_do_operation = false;
            bool b_negative = false;
            char znak;
            sbyte new_signum;
            
            if (this.signum == b.signum)
            {
                new_signum = 1;
            }
            else
            {
                new_signum = -1;
            }

            if(b.signum == -1)
            {
                b = b.NegativeNumber();
                b_negative = true;
            }

            if (this.signum == -1)
            {
                znak = this.NegativeNumber().Comparison(b);
            }
            else
            {
                znak = this.Comparison(b);
            }
            
            if (znak == '<')
            {
                answer = new MyBigInteger("0");
                ostatok = this;
                return;
            }
            else
            {
                if (znak == '=')
                {
                    answer = new MyBigInteger("1");
                    ostatok = new MyBigInteger("0");
                    return;
                }
            }


            for (int i = 0; i < b.PartSize(); i++) 
            {
                now_numb.number.Insert(0, this.number[temp_i--]);
            }

            do
            {
                flag = false;
                while (now_numb.Comparison(b) == '<')
                {
                    if (temp_i >= 0)
                    {
                        if (flag_do_operation)
                        {
                            flag_do_operation = false;
                        }
                        else
                        {
                            numbers_answer.Insert(0, 0);
                        }
                        
                        now_numb.number.Insert(0, this.number[temp_i--]);
                    }
                    else
                    {
                        numbers_answer.Insert(0, 0);
                        ostatok = now_numb;
                        answer = new MyBigInteger(new_signum, numbers_answer);
                        return;
                    }               
                }

                down = 0;
                up = basis;
                while (up > down + 1)
                {
                    int temp = (down + up) / 2;
                    temp_bigInt = b.Multiply(new MyBigInteger(temp.ToString()));
                    znak = temp_bigInt.Comparison(now_numb);

                    if (znak == '>')
                    {
                        up = temp;
                    }
                    else
                    {
                        if (znak == '<')
                        {
                            down = temp;
                        }
                        else
                        {
                            numbers_answer.Insert(0, temp);
                            flag = true;
                            now_numb.number.Clear();
                            break;
                        }
                    }
                }

                if (!flag)
                {
                    numbers_answer.Insert(0, down);
                    now_numb = now_numb.Subtract(b.Multiply(new MyBigInteger(down.ToString())));
                }

                flag_do_operation = true;

                int count = now_numb.number.Count();
                for (int j = count - 1; j >= 0; j--)
                {
                    if (now_numb.number[j] == 0)
                    {
                        now_numb.number.RemoveAt(j);
                    }
                    else
                    {
                        break;
                    }
                }
            } 
            while (temp_i >= 0);

            answer = new MyBigInteger(new_signum, numbers_answer);
            if (b_negative)
            {
                b = b.NegativeNumber();
            }
            ostatok = this.Subtract(b.Multiply(answer));
        }

        public bool IsZero()
        {
            if (number.Count == 1 && number[0] == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}