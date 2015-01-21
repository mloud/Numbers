using System.Collections;
using System.Collections.Generic;
using System;


public static class SpecialityFactory
{
    public static Speciality Create(string specName, string param)
    {
        Speciality spec = null;
        if (specName == "m")
        {
            spec = new MinusOneSpeciality();
        }
        else if (specName == "r")
        {
            spec = new ReappearSpeciality();
        }

        if (spec != null)
        {
            spec.Init(param);
        }

        return spec;
    }
}
