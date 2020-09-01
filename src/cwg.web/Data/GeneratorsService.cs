using System;
using System.Linq;

using cwg.web.Generators;
using cwg.web.Repositories;

namespace cwg.web.Data
{
    public class GeneratorsService
    {
        private BaseGenerator GetGenerator(string name) => GeneratorRepository.GetGenerators().FirstOrDefault(a => a.Name == name);

        public GenerationResponseModel GenerateFile(GenerationRequestModel model)
        {
            try
            {
                var generator = GetGenerator(model.FileType);

                if (generator == null)
                {
                    throw new Exception($"{model.FileType} was not found");
                }

                var (sha1, fileName) = generator.GenerateFiles(model.NumberToGenerate);

                return new GenerationResponseModel
                {
                    FileName = fileName,
                    SHA1 = sha1,
                    FileType = model.FileType
                };
            } catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"Failed to GenerateFile ({model.FileType}): {ex}");

                return null;
            }
        }
    }
}