using System;
using System.Linq;
using System.Xml.Linq;

enum Method
{
    Normal,
    Express,
}

enum LetterFormat
{
    A3,
    A4,
}

class Mail
{
    private double weight;
    private Method shippingMethod;
    private String destinationAddress;
    private bool validMail;

    public Mail()
    {
        this.weight = 0;
        this.shippingMethod = Method.Normal;
        this.destinationAddress = "";
        this.validMail = false;
    }

    public double Weight
    {
        get { return this.weight; }
        set { this.weight = value; }
    }

    public Method ShippingMethod
    {
        get { return this.shippingMethod; }
        set { this.shippingMethod = value; }
    }

    public String DestinationAddress
    {
        get { return this.destinationAddress; }
        set { this.destinationAddress = value; }
    }

    public bool ValidMail
    {
        get { return this.validMail; }
        set { this.validMail = value; }
    }

    public virtual double stamp()
    {
        return 0.0;
    }

    public virtual void validate()
    {
        if (!String.Equals(destinationAddress, "", StringComparison.OrdinalIgnoreCase))
        {
            this.validMail = true;
        }
    }
}

class Letter : Mail
{
    private LetterFormat format;

    public Letter(double paramWeight, Method paramShippingMethod, String paramDestinationAddress, LetterFormat paramLetterFormat)
    {
        this.Weight = paramWeight;
        this.ShippingMethod = paramShippingMethod;
        this.DestinationAddress = paramDestinationAddress;
        this.format = paramLetterFormat;
    }

    public LetterFormat Format
    {
        get { return this.format; }
        set { this.format = value; }
    }

    public override double stamp()
    {
        double dBaseFare = (this.format == LetterFormat.A3) ? 3.50 : 2.50;
        double dAmount = dBaseFare + (1.0 * (this.Weight / 1000));
        if (this.ShippingMethod == Method.Express)
        {
            dAmount *= dAmount; 
        }

        return dAmount;
    }

    public override string ToString()
    {
        return "Letter: \nShipping Address: " + this.DestinationAddress + "\nShipping Method: " + this.ShippingMethod + "\nFormat: " + this.format + "\nWeight: " + this.Weight;
    }
}

class Parcel : Mail
{
    private double volume;

    public Parcel(double paramWeight, Method paramShippingMethod, String paramDestinationAddress, double paramVolume)
    {
        this.Weight = paramWeight;
        this.ShippingMethod = paramShippingMethod;
        this.DestinationAddress = paramDestinationAddress;
        this.volume = paramVolume;
    }

    public double Volume
    {
        get { return this.volume; }
        set { this.volume = value; }
    }

    public override double stamp()
    {
        double dAmount = 0.0;
        if (this.volume !> 50) {
            dAmount = ((0.25 * this.volume) + (this.Weight / 1000)) * 1.0;
            if (this.ShippingMethod == Method.Express)
            {
                dAmount *= dAmount;
            }
        }

        return dAmount;
    }

    public override void validate()
    {
        if (!(String.Equals(this.DestinationAddress, "", StringComparison.OrdinalIgnoreCase)) && (volume <= 50))
        {
            this.ValidMail = true;
        }
    }

    public override string ToString()
    {
        return "Parcel: \nShipping Address: " + this.DestinationAddress + "\nShipping Method: " + this.ShippingMethod + "\nVolume: " + this.volume + "\nWeight: " + this.Weight;
    }
}

class Advertisement : Mail
{
    public Advertisement(double paramWeight, Method paramShippingMethod, String paramDestinationAddress)
    {
        this.Weight = paramWeight;
        this.ShippingMethod = paramShippingMethod;
        this.DestinationAddress = paramDestinationAddress;
    }

    public override double stamp()
    {

        double dAmount = 5.0 * (this.Weight / 1000);
        if (this.ShippingMethod == Method.Express)
        {
            dAmount *= dAmount;
        }

        return dAmount;
    }

    public override string ToString()
    {
        return "Advertisement: \nShipping Address: " + this.DestinationAddress + "\nShipping Method: " + this.ShippingMethod + "\nWeight: " + this.Weight;
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Mail[] arrMailBox =
        {
            new Letter(25, Method.Normal, "Philippines", LetterFormat.A3),
            new Letter(5, Method.Normal, "Colombia", LetterFormat.A4),
            new Letter(50, Method.Normal, "", LetterFormat.A4),
            new Parcel(500, Method.Normal, "", 10),
            new Parcel(100, Method.Normal, "Japan", 45),
            new Parcel(200, Method.Normal, "Florida", 30),
            new Advertisement(25, Method.Normal, ""),
            new Advertisement(25, Method.Normal, ""),
            new Advertisement(25, Method.Normal, "Thailand"),
        };

        Console.WriteLine(String.Format("Invalid Mail: {0} of {1}", invalidMails(arrMailBox), arrMailBox.Length));
        display(arrMailBox);

        int invalidMails(Mail[] paramMailBox)
        {
            int iCount = 0;
            foreach (Mail mail in paramMailBox)
            {
                mail.validate();
                if (!mail.ValidMail)
                {
                    iCount += 1;
                }
            }

            return iCount;
        }

        void display(Mail[] paramMailBox)
        {
            foreach (Mail mail in paramMailBox)
            {
                Console.WriteLine(String.Format("{0}\n", mail.ToString()));
            }
        }
    }
}
