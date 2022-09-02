# StructuredLoggingConverter
Application converts interpolated logs to structured logs.

```
  -p, --path            [Required] Path

  --useDefaultNames     If enabled program will not ask about names for arguments, and will use default ones.

  --generateNewFiles    If enabled program will generate new files instead of replacing old ones.
```
  
Program iterates over all interpolated logs and asks about names for log paramaters. Then it generates structured log and put it in place of old log. 
Example:
- For log: ```_logger.LogDebug($"Starting cross scan - data PreProcessing {preProcessResults}");```
- Program asks about name for preProcessResults parameter. It uses original name as a placeholder ("PreProcessResults" in this case), but it could be changed to something else. In this example I used "PreProcessData".
- Program replaces old log with new one:
		```_logger.LogDebug("Starting cross scan - data PreProcessing {PreProcessData}",
			new { PreProcessData = preProcessResults });```
			
If the log doesn't have any interpolated parameters, but use interpolation sign ($) the sign  will be deleted automatically, without asking about anything. Example
  - For log: ```_logger.LogDebug($"Building X, Y data for the Linear Fit Calculator finished successfully");```
  - Program generates new log: ```_logger.LogDebug("Building X, Y data for the Linear Fit Calculator finished successfully");```
		
		
		
Even if program takes care of most log instruction it still needed to modify some of them manually.
- Some of the logs use parameters, but doesn't reference them in the template. For example:
			```_logger.LogDebug("clone updated", bridgeSurfaceRelationClone);```
		As a general rule each parameter should be referenced in template, so it should be changed to:
			```_logger.LogDebug("clone updated {BridgeSurfaceRelationClone}", bridgeSurfaceRelationClone);```
		Such logs can be easily found with following regex:
			```Log(Debug|Error|Information|Trace|Warning|Critical)\("[^\{\}]+".*,```
- If log contains exception it should be placed as first argument, and not as one of many parameters, for example this log is wrong:
			```_log.LogError("Failed to convert from KSF to Job {Exception}", ex);```
		And should be replaced with this:
			```_log.LogError(ex, "Failed to convert from KSF to Job");```
	

      
