using System.Text.Json.Serialization;

namespace Saga.Orchestrator.Core.DomainObjects
{
    public record struct Cpf
    {
        public const int CpfMaxLength = 11;
        public readonly string Value { get; }

        [JsonConstructor]
        public Cpf(string value)
        {
            Value = value;
        }

        public static bool Validar(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            cpf = GetOnlyNumbers(cpf?.ToString() ?? string.Empty);

            if (cpf.Length > CpfMaxLength)
                return false;

            while (cpf.Length < CpfMaxLength)
                cpf = '0' + cpf;

            if (AllDigitsEquals(cpf))
                return false;

            var multiplier1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplier2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);

            var soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplier1[i];
            }

            var result = soma % CpfMaxLength;
            result = result < 2 ? 0 : CpfMaxLength - result;

            var digit = result.ToString();
            tempCpf = tempCpf + digit;

            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplier2[i];
            }

            result = soma % CpfMaxLength;
            result = result < 2 ? 0 : CpfMaxLength - result;

            digit = result.ToString();
            tempCpf = tempCpf + digit;

            return cpf.EndsWith(tempCpf);
        }

        private static string GetOnlyNumbers(string cpf)
        {
            return new string(cpf.Where(char.IsDigit)?.ToArray());
        }

        private static bool AllDigitsEquals(string cpf)
        {
            var first = cpf[0];
            return cpf.All(d => d == first);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
