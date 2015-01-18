using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public abstract class BonusBase : MonoBehaviour
{

	public abstract void Consume();

	public abstract string GetText();
}
