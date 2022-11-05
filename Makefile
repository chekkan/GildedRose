test:
	dotnet test --logger "console;verbosity=detailed"\
		--collect "XPlat Code Coverage"\
		-- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=lcov