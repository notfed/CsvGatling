using System;
using System.Reflection;
using System.Reflection.Emit;

namespace CsvGatling
{
    /// <summary>
    /// Creates a factory 
    /// </summary>
    public class DynamicCsvRowActivatorFactoryBuilder<T>
    {
        private readonly IConverterProvider converterProvider;
        private readonly IColumnNameMapper columnNameMapper;

        public DynamicCsvRowActivatorFactoryBuilder()
            : this(new DefaultConverterProvider(), new DefaultColumnNameMapper())
        {
        }
        public DynamicCsvRowActivatorFactoryBuilder(IConverterProvider converterProvider, IColumnNameMapper columnNameMapper)
        {
            if (converterProvider == null)
            {
                throw new ArgumentNullException("converterProvider");
            }
            if (columnNameMapper == null)
            {
                throw new ArgumentNullException("columnNameMapper");
            }
            this.converterProvider = converterProvider;
            this.columnNameMapper = columnNameMapper;
        }

        public TypeBuilder Build(AssemblyBuilder assembly, ModuleBuilder module, ConstructorInfo constructorOfActivatorT)
        {
            TypeBuilder factoryTypeBuilder = BuildClass(module);
            MethodBuilder methodCreate = BuildMethodCreate(factoryTypeBuilder, constructorOfActivatorT);
            return factoryTypeBuilder;
        }

        private TypeBuilder BuildClass(ModuleBuilder module)
        {
            TypeBuilder type = module.DefineType(
                "TryDynamicCsvGatling.DynamicCsvReaderActivatorFactory", 
                TypeAttributes.Public
                | TypeAttributes.Class
                | TypeAttributes.AutoClass
                | TypeAttributes.AnsiClass
                | TypeAttributes.BeforeFieldInit
                | TypeAttributes.AutoLayout, 
                typeof(Object),   
                new Type[]{
                    typeof(ICsvRowActivatorFactory<T>)
                    }
                );
            return type;
        }

        private MethodBuilder BuildMethodCreate(
            TypeBuilder classBuilder,
            ConstructorInfo constructorOfActivatorT)
        {
            // ICsvRowActivator<T> Create(string[] headerrow)
            // {
            MethodBuilder method = classBuilder.DefineMethod("Create",
                MethodAttributes.Public
               | MethodAttributes.HideBySig
               | MethodAttributes.NewSlot
               | MethodAttributes.Virtual
               | MethodAttributes.Final,
               CallingConventions.HasThis,
               typeof(ICsvRowActivator<T>),
               new Type[] { typeof(string[]) }
            );

            ILGenerator gen = method.GetILGenerator();

            // ICsvReaderActivator<J139Report> obj = new DynamicCsvReaderActivator<J139Report>(headerrow);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Newobj, constructorOfActivatorT);

            // }
            gen.Emit(OpCodes.Ret);


            return method;
        }
    }
}
