# Boiler Controller Application

## 1. Application Initialization:
• As the application starts, display a welcoming message: "Boiler Controller 
Initialized."

• Initialize the boiler's system status to "Lockout."

• Initiate the event log, capturing all significant events. The first log entry should 
be "Boiler Initialized" with the current timestamp. (Refer to “Event Log File 
Specification” for complete details)

## 2. Console Interface Presentation:
• Present a clear menu to the user, making it intuitive to navigate between 
operations:

• Start Boiler Sequence

• Stop Boiler Sequence

• Simulate Boiler Error. (Can be used to Simulate error only when the 
boiler is in Operational mode)

• Toggle Run Interlock Switch (Open/Closed)

• Reset Lockout

• View Event Log

• Exit Application

## 3. Run Interlock Simulation:
• This is a safety feature. At initialization, the switch is in the "Open" position.

• The user must be able to toggle this switch.

• The boiler can only be ready or started if the switch is in the "Closed" position.

• Each time the switch state changes, log the event with a message like "Interlock 
Switch toggled to [state]."

## 4. Lockout Reset Functionality:
• Reset is essential after any failure or after initialization.
• If the "Run Interlock" switch is in the "Closed" position, the system status should 
transition to "Ready." Else, inform the user of the need to close the interlock 
switch.
• Log the event: "Boiler Status changed to Ready" with the timestamp.

## 5. Boiler Start Sequence:
• This sequence simulates the various phases of a boiler startup.

### Pre-Purge Cycle:
• Lasts for 10 seconds.

• Status: "Pre-Purge"

• Users should see a countdown or elapsed time.

• At the end, log: "Pre-Purge completed."

### Ignition Phase:
• Lasts for 10 seconds.

• Status: "Ignition"

• Similarly, provide a time indication for the user.

• Log: "Ignition phase completed."

### Operational State:

• After ignition, transition to "Operational."

• Log: "Boiler now operational." Continue to be in this state.

## 6. Event Logging Mechanism:
• Capture all significant events in an event log (A file on disk).

• Each log entry should have a timestamp, event name, and relevant data (if any).

• Provide an option for the user to view this log, which should display the entries 
in a clear, readable format.

## 7. Safety and Error Handling:
• If there is any simulated failure or error during a phase, transition back to the 
"Lockout" state.

• Display a clear error message: "Error: [Error Description]. System in Lockout."

• Log the error with its timestamp.

