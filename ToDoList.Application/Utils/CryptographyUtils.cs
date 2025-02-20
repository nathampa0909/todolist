using System.Security.Cryptography;
using System.Text;

namespace ToDoList.Application.Utils;

public static class CryptographyUtils
{
	public static string GetSHA256HashFromString(string password)
	{
		using var sha256 = SHA256.Create();
		var bytes = Encoding.UTF8.GetBytes(password);
		var hash = sha256.ComputeHash(bytes);

		return Convert.ToHexStringLower(hash);
	}
}
