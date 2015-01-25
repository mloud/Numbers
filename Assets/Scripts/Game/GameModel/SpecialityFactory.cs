using System.Collections;
using System.Collections.Generic;
using System;


public static class SpecialityFactory
{
    public static Speciality Create(string specName, string param, CircleController circle, GameContext context)
    {
        Speciality spec = null;
        if (specName == "m")
        {
            spec = new MinusOneSpeciality(circle, context);
        }
        else if (specName == "r")
        {
            spec = new ReappearSpeciality(circle, context);
        }
        else if (specName == "f")
        {
            spec = new ChangeValueSpeciality(circle, context);
        }

        if (spec != null)
        {
            spec.Init(param);
        }

        return spec;
    }
}
