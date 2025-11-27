using ClosedXML.Excel;
using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.Repositories;
using Google;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace FactorPolar.CargaEmpleados
{
    public class ConsoleAppService
    {
        private readonly IEmployee _empRepository;
        private readonly IBeneficiario _benefiRepository;

        public ConsoleAppService(IEmployee empRepository, IBeneficiario benefiRepository) // Inyección del repositorio
        {
            _empRepository = empRepository;
            _benefiRepository = benefiRepository;
        }

        public Task RunAsync()
        {
            Console.WriteLine("Iniciando Proceso de Carga!");

            // Obtener la ruta del directorio del ejecutable
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Obtener todos los archivos .xlsx en ese directorio
            string[] excelFiles = Directory.GetFiles(exeDir, "*.xlsx");

            foreach (string filePath in excelFiles)
            {
                Console.WriteLine($"Procesando archivo: {filePath}");

                try
                {
                    ReadDataFileEmloyee(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al procesar el archivo: {filePath} - {ex.Message}");
                }

                //File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        async Task<bool> ReadDataFileEmloyee(string filePath)
        {
            try
            {
                var workRead = new XLWorkbook(filePath);
                var sheetRead = workRead.Worksheets.Where(x => x.Name == "Población_incumbente").First();
                string empleadomail = string.Empty;
                string empleadoaux = string.Empty;

                string campos = string.Empty;
                int vacios = 0;

                for (int i = 2; i < 20000; i++)
                {
                    if (string.IsNullOrEmpty(sheetRead.Cell("AK" + i).GetString().Trim()))
                    {
                        vacios++;
                        if (vacios > 3)
                            break;

                        continue;
                    }

                    empleadomail = sheetRead.Cell("AK" + i).GetString().Trim();
                    if (empleadomail != empleadoaux)
                    {
                        //Employee emplo = await _userRepository.GetByEmail(empleadomail);
                        //if (emplo != null)
                        //    continue;

                        Console.WriteLine($"Procesando empleado: {empleadomail}");
                        empleadoaux = empleadomail;
                        Employee employee = new Employee();
                        employee.Email = sheetRead.Cell("AK" + i).GetString().Trim();
                        employee.EmployeeNumber = int.Parse(sheetRead.Cell("A" + i).GetString().Trim());
                        employee.FullName = sheetRead.Cell("B" + i).GetString().Trim();
                        employee.Gender = sheetRead.Cell("O" + i).GetString().Trim();
                        employee.Status = sheetRead.Cell("D" + i).GetString().Trim();
                        employee.CodigoTipo = sheetRead.Cell("E" + i).GetString().Trim();
                        employee.DescipcionTipo = sheetRead.Cell("F" + i).GetString().Trim();
                        employee.CreateDate = DateTime.Now;
                        employee.UpdateDate = DateTime.Now;

                        Console.WriteLine($"Agregando empleado: {empleadomail}");
                        _empRepository.Create(employee);
                        Console.WriteLine($"Agregado exitosamente: {empleadomail}");
                    }

                    Console.WriteLine($"Procesando beneficiario: {sheetRead.Cell("W" + i).GetString().Trim()}");

                    Beneficiario beneficiario = new Beneficiario();
                    beneficiario.Denominacion = sheetRead.Cell("U" + i).GetString().Trim();
                    beneficiario.CedulaIdentidad = sheetRead.Cell("V" + i).GetString().Trim();
                    beneficiario.FullName = sheetRead.Cell("W" + i).GetString().Trim();
                    beneficiario.Gender = sheetRead.Cell("Z" + i).GetString().Trim();
                    beneficiario.FechaNacimiento = DateTime.Parse(sheetRead.Cell("AA" + i).GetString().Trim());
                    beneficiario.Edad = int.Parse(sheetRead.Cell("AB" + i).GetString().Trim());
                    beneficiario.TipoInstitucion = sheetRead.Cell("AC" + i).GetString().Trim();
                    beneficiario.NombreInstitucion = sheetRead.Cell("AE" + i).GetString().Trim();
                    beneficiario.RifInstitucion = sheetRead.Cell("AF" + i).GetString().Trim();
                    beneficiario.NivelEducativo = sheetRead.Cell("AG" + i).GetString().Trim();
                    beneficiario.GradoEducativo = sheetRead.Cell("AH" + i).GetString().Trim();
                    beneficiario.CreateDate = DateTime.Now;
                    beneficiario.UpdateDate = DateTime.Now;

                    Console.WriteLine($"Agregando beneficiario: {sheetRead.Cell("W" + i).GetString().Trim()}");
                    _benefiRepository.Create(beneficiario, empleadomail);
                    Console.WriteLine($"Agregado exitosamente: {sheetRead.Cell("W" + i).GetString().Trim()}");
                }

                //for (int i = 2; i < 20000; i++)
                //{
                //    if (string.IsNullOrEmpty(sheetRead.Cell("AK" + i).GetString().Trim()))
                //    {
                //        vacios++;
                //        if (vacios > 3)
                //            break;

                //        continue;
                //    }

                //    empleadomail = sheetRead.Cell("AK" + i).GetString().Trim();
                //    //Employee employee = await _userRepository.GetByEmail(empleadomail);
                //    //if (employee == null)
                //    //    continue;

                //    Console.WriteLine($"Procesando beneficiario: {sheetRead.Cell("W" + i).GetString().Trim()}");

                //    Beneficiario beneficiario = new Beneficiario();
                //    beneficiario.Denominacion = sheetRead.Cell("U" + i).GetString().Trim();
                //    beneficiario.CedulaIdentidad = sheetRead.Cell("V" + i).GetString().Trim();
                //    beneficiario.FullName = sheetRead.Cell("W" + i).GetString().Trim();
                //    beneficiario.Gender = sheetRead.Cell("Z" + i).GetString().Trim();
                //    beneficiario.FechaNacimiento = DateTime.Parse(sheetRead.Cell("AA" + i).GetString().Trim());
                //    beneficiario.Edad = int.Parse(sheetRead.Cell("AB" + i).GetString().Trim());
                //    beneficiario.TipoInstitucion = sheetRead.Cell("AC" + i).GetString().Trim();
                //    beneficiario.NombreInstitucion = sheetRead.Cell("AE" + i).GetString().Trim();
                //    beneficiario.RifInstitucion = sheetRead.Cell("AF" + i).GetString().Trim();
                //    beneficiario.NivelEducativo = sheetRead.Cell("AG" + i).GetString().Trim();
                //    beneficiario.GradoEducativo = sheetRead.Cell("AH" + i).GetString().Trim();
                //    beneficiario.CreateDate = DateTime.Now;
                //    beneficiario.UpdateDate = DateTime.Now;
                //    _benefiRepository.Create(beneficiario, empleadomail);
                //}

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
