using System;
using System.Reflection;
using System.Reflection.Emit;

namespace CsvGatling
{
    public class DynamicCsvRowActivatorFactoryFactory<T> : ICsvRowActivatorFactoryFactory<T>
    {
        public ICsvRowActivatorFactory<T> Create(IConverterProvider converterProvider, IColumnNameMapper columnNameMapper)
        {
            // Create assembly/module
            bool saveDll = false;
            AssemblyName assemblyName = new AssemblyName("DynamicCsvRowActivator");
            AssemblyBuilder assemblyBuilder = BuildAssembly(assemblyName, new Version(1, 0, 0, 0), saveDll);
            ModuleBuilder moduleBuilder = BuildModule(assemblyBuilder, assemblyName, saveDll);

            // Create activator type
            DynamicCsvRowActivatorBuilder<T> activatorBuilder
                = new DynamicCsvRowActivatorBuilder<T>(converterProvider, columnNameMapper);
            
            TypeBuilder activatorTypeBuilder = activatorBuilder.Build(assemblyBuilder, moduleBuilder);
            Type activatorType = activatorTypeBuilder.CreateType();
            ConstructorInfo activatorConstructor = activatorType.GetConstructor(new Type[] { typeof(string[]) });

            // Create factory type
            DynamicCsvRowActivatorFactoryBuilder<T> factoryBuilder
                = new DynamicCsvRowActivatorFactoryBuilder<T>();
            TypeBuilder typeBuilder = factoryBuilder.Build(assemblyBuilder, moduleBuilder, activatorConstructor);
            Type factoryType = typeBuilder.CreateType();
            ConstructorInfo factoryConstructor = factoryType.GetConstructor(Type.EmptyTypes);

            // Save the dll
            if (saveDll)
            {
                assemblyBuilder.Save("DynamicCsvRowActivator.dll");
            }

            // Create an instance of a factory 
            ICsvRowActivatorFactory<T> factory = (ICsvRowActivatorFactory<T>)factoryConstructor.Invoke(new object[] { });
            return factory;
        }
        private AssemblyBuilder BuildAssembly(AssemblyName assemblyName, Version version, bool saveDll)
        {
            AppDomain domain = AppDomain.CurrentDomain;
            assemblyName.Version = version;
            if (saveDll)
            {
                return domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave, @"C:\Temp\");
            }
            else
            {
                return domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            }
        }
        private ModuleBuilder BuildModule(AssemblyBuilder assembly, AssemblyName assemblyName, bool saveDll)
        {
            if (saveDll)
            {
                return assembly.DefineDynamicModule(assemblyName + "Module", assemblyName + "Module.dll", true);
            }
            else
            {
                return assembly.DefineDynamicModule(assemblyName + "Module");
            }
        }
    }
}
