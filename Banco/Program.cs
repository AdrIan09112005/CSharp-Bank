// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;

namespace Banco_Electronico
{
    internal class Program
    {
        // Modelo de cuenta bancaria
        class Cuenta
        {
            public string NumeroTarjeta { get; set; }
            public string Pin { get; set; }
            public decimal Saldo { get; set; } = 0;
            public bool TarjetaVencida { get; set; } = false;
        }

        static List<Cuenta> cuentas = new();
        static Cuenta cuentaActual = null;

        static void Main(string[] args)
        {
            Console.WriteLine("************************");
            Console.WriteLine("C#Bank - Cajero Virtual");
            Console.WriteLine("************************");

            RegistrarTarjetasIniciales();

            while (true)
            {
                Console.WriteLine("1. Iniciar sesión");
                Console.WriteLine("2. Registrar nueva tarjeta");
                Console.WriteLine("3. Salir");
                Console.Write("Opción: ");
                string op = Console.ReadLine();
                Console.WriteLine("****************************");

                switch (op)
                {
                    case "1":
                        IniciarSesion();
                        break;
                    case "2":
                        RegistrarTarjeta();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            }
        }

        static void RegistrarTarjetasIniciales()
        {
            // Tarjeta de ejemplo para pruebas
            cuentas.Add(new Cuenta { NumeroTarjeta = "123456789", Pin = "1234", Saldo = 1000 });
        }

        static void RegistrarTarjeta()
        {
            Console.Write("Número de tarjeta: ");
            string numero = Console.ReadLine();

            Console.Write("PIN: ");
            string pin = Console.ReadLine();

            if (numero.Length != 9 || pin.Length != 4 || !numero.All(char.IsDigit) || !pin.All(char.IsDigit))
            {
                Console.WriteLine("Formato inválido. La tarjeta debe tener 9 dígitos y el PIN 4 dígitos.");
                return;
            }

            if (cuentas.Any(c => c.NumeroTarjeta == numero))
            {
                Console.WriteLine("La tarjeta ya está registrada.");
                return;
            }

            cuentas.Add(new Cuenta { NumeroTarjeta = numero, Pin = pin });
            Console.WriteLine("Tarjeta registrada correctamente.");
            Console.WriteLine("****************************");
        }

        static void IniciarSesion()
        {
            Console.Write("Número de tarjeta: ");
            string numero = Console.ReadLine();
            Console.Write("PIN: ");
            string pin = Console.ReadLine();

            cuentaActual = cuentas.FirstOrDefault(c => c.NumeroTarjeta == numero && c.Pin == pin);

            if (cuentaActual != null)
            {
                Console.Clear();
                Menu();
            }
            else
            {
                Console.WriteLine("Credenciales incorrectas.");
            }
        }

        static void Menu()
        {
            char op;
            do
            {
                Console.WriteLine("************************");
                Console.WriteLine("C#Bank - Cajero Virtual");
                Console.WriteLine("************************");
                Console.WriteLine("Selecciona una opción: ");
                Console.WriteLine("1. Retiro de Dinero");
                Console.WriteLine("2. Transferencia de Fondos");
                Console.WriteLine("3. Deposito de Fondos");
                Console.WriteLine("4. Consulta de Datos");
                Console.WriteLine("s. SALIR");
                Console.WriteLine("****************************");
                op = char.Parse(Console.ReadLine());

                switch (op)
                {
                    case '1':
                        RetiroDinero();
                        Console.WriteLine("Pulsa cualquier tecla para regresar al Menu...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case '2':
                        TranferenciaDinero();
                        Console.WriteLine("Pulsa cualquier tecla para regresar al Menu...");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case '3':
                        DepositoFondos();
                        Console.WriteLine("Pulsa cualquier tecla para regresar al Menu");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case '4':
                        ConsultaDatos();
                        Console.WriteLine("Pulse cualquier tecla para regresar al Menu");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case 's':
                        cuentaActual = null;
                        return;
                    default:
                        Console.WriteLine("No se reconoce la opción");
                        break;
                }

            } while (op != 's');
        }

        static void RetiroDinero()
        {
            Console.WriteLine("*********************************");
            Console.WriteLine("Bienvenido. ¿Desea retirar dinero?");
            Console.WriteLine("*********************************");
            Console.WriteLine("S/N");
            char option = char.Parse(Console.ReadLine().ToUpper());

            if (option != 'S') return;

            Console.WriteLine("Ingrese el monto a retirar:");
            decimal montoRetiro = decimal.Parse(Console.ReadLine());

            if (cuentaActual.TarjetaVencida)
            {
                Console.WriteLine("Tarjeta vencida. Operación no permitida.");
            }
            else if (montoRetiro <= cuentaActual.Saldo)
            {
                cuentaActual.Saldo -= montoRetiro;
                Console.WriteLine("Retiro exitoso. Nuevo saldo: " + cuentaActual.Saldo);
            }
            else
            {
                Console.WriteLine("Fondos insuficientes.");
            }
        }

        static void TranferenciaDinero()
        {
            Console.WriteLine("Ingrese el número de cuenta destino:");
            string destino = Console.ReadLine();

            Console.WriteLine("Ingrese el monto a transferir:");
            decimal montoTransferencia = decimal.Parse(Console.ReadLine());

            var cuentaDestino = cuentas.FirstOrDefault(c => c.NumeroTarjeta == destino);

            if (cuentaDestino == null)
            {
                Console.WriteLine("La cuenta destino no existe.");
                return;
            }

            if (montoTransferencia <= cuentaActual.Saldo)
            {
                cuentaActual.Saldo -= montoTransferencia;
                cuentaDestino.Saldo += montoTransferencia;
                Console.WriteLine("Transferencia exitosa. Nuevo saldo: " + cuentaActual.Saldo);
            }
            else
            {
                Console.WriteLine("No se pudo realizar la transferencia. Fondos insuficientes.");
            }
        }

        static void DepositoFondos()
        {
            Console.WriteLine("Ingrese el número de tarjeta a depositar:");
            string numeroTarjetaDeposito = Console.ReadLine();

            Console.WriteLine("Ingrese el monto a depositar:");
            decimal montoDeposito = decimal.Parse(Console.ReadLine());

            var cuentaDestino = cuentas.FirstOrDefault(c => c.NumeroTarjeta == numeroTarjetaDeposito);

            if (cuentaDestino != null && montoDeposito > 0)
            {
                cuentaDestino.Saldo += montoDeposito;
                Console.WriteLine("Depósito exitoso. Nuevo saldo de la cuenta destino: " + cuentaDestino.Saldo);
            }
            else
            {
                Console.WriteLine("No se pudo realizar el depósito. Verifique los datos ingresados.");
            }
        }

        static void ConsultaDatos()
        {
            if (cuentaActual.TarjetaVencida)
            {
                Console.WriteLine("Su plástico ha vencido, pase a ventanilla para obtener uno nuevo.");
            }
            else
            {
                Console.WriteLine("Bienvenido");
                Console.WriteLine("Número de tarjeta: " + cuentaActual.NumeroTarjeta);
                Console.WriteLine("Saldo actual: " + cuentaActual.Saldo);
            }
        }
    }
}
