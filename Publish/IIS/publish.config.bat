xcopy Config sites\Beeant.Cloud.Welfare.Admin\Config 			/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Cloud.Welfare.Api\Config 			/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Cloud.Welfare.Mobile\Config 			/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Cloud.Welfare.Website\Config 			/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Distributed.Outside.Document\Config 		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Distributed.Outside.Image\Config 		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Admin.Erp\Config 		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Admin.Home\Config 		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Mobile.Login\Config 		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Mobile.Login\Config 		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Website.Home\Config 		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Website.Password\Config 		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Website.Register\Config  	/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Website.Login\Config  		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Website.Shared\Config  		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Distributed.Inside.Message\Config  		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Website.Editor\Config  		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config sites\Beeant.Presentation.Admin.Editor\Config  		/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config host\ICacheService\Config  				/y /e /v /i /s   /exclude:_exclude.file.txt
xcopy Config host\IDocFileService\Config  				/y /e /v /i /s   /exclude:_exclude.file.txt 
xcopy Config host\IIdentityService\Config  				/y /e /v /i /s   /exclude:_exclude.file.txt 
xcopy Config host\IImageFileService\Config  				/y /e /v /i /s   /exclude:_exclude.file.txt 
xcopy Config host\IImageTempFileService\Config  			/y /e /v /i /s   /exclude:_exclude.file.txt 
xcopy Config host\IQueueService\Config  				/y /e /v /i /s   /exclude:_exclude.file.txt 
xcopy Config host\IRegisterService\Config  				/y /e /v /i /s   /exclude:_exclude.file.txt 
xcopy Config host\ISearchService\Config  				/y /e /v /i /s   /exclude:_exclude.file.txt 
iisreset
start host\IRegisterService\Beeant.Distributed.Service.Host.exe
start host\IIdentityService\Beeant.Distributed.Service.Host.exe
start host\ICacheService\Beeant.Distributed.Service.Host.exe
start host\IDocFileService\Beeant.Distributed.Service.Host.exe
start host\IImageFileService\Beeant.Distributed.Service.Host.exe
start host\IQueueService\Beeant.Distributed.Service.Host.exe
start host\ISearchService\Beeant.Distributed.Service.Host.exe
start host\IImageTempFileService\Beeant.Distributed.Service.Host.exe

pause