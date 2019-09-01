using UnityEngine;

public static class QuaternionExtensions
{
	public static Quaternion Pow(this Quaternion input, float power)
	{
		float inputMagnitude = input.Magnitude();
		Vector3 nHat = new Vector3(input.x, input.y, input.z).normalized;
		Quaternion VectorBit = new Quaternion(nHat.x, nHat.y, nHat.z, 0)
			.ScalarMultiply(power * Mathf.Acos(input.w / inputMagnitude))
				.Exp();
		return VectorBit.ScalarMultiply(Mathf.Pow(inputMagnitude, power));
	}
 
	public static Quaternion Exp(this Quaternion input)
	{
		float inputa = input.w;
		Vector3 inputv = new Vector3(input.x, input.y, input.z);
		float outputa = Mathf.Exp(inputa) * Mathf.Cos(inputv.magnitude);
		Vector3 outputv = Mathf.Exp(inputa) * (inputv.normalized * Mathf.Sin(inputv.magnitude));
		return new Quaternion(outputv.x, outputv.y, outputv.z, outputa);
	}
 
	public static float Magnitude(this Quaternion input)
	{
		return Mathf.Sqrt(input.x * input.x + input.y * input.y + input.z * input.z + input.w * input.w);
	}
 
	public static Quaternion ScalarMultiply(this Quaternion input, float scalar)
	{
		return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
	}

	/// Normalize the Quaternion.
	public static Quaternion GetNormalized(this Quaternion q)
	{
		float f = 1f / Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
		return new Quaternion(q.x*f, q.y*f, q.z*f, q.w*f);
	}
}