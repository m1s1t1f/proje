#include <StepperMotor.h>
 
StepperMotor motor(8,9,10,11);
String a;



int b;


void setup(){
  Serial.begin(9600);
  motor.setStepDuration(1);
  
}
 
void loop()
{
if (Serial.available())
    {
      a = Serial.readString();
      int str_len = a.length() + 1; 
      char char_array[str_len];
      a.toCharArray(char_array, str_len);
      b = atoi(char_array);        
    }
        if(b == 3)
        {
          motor.step(50);
          delay(5);   
        }
        if (b==1)
        {
          sag();  
        }
        if (b==2)
        {
          digitalWrite(8, LOW);
          delay(5);
          digitalWrite(9, LOW);
          delay(5);
          digitalWrite(10, LOW);
          delay(5);
          digitalWrite(11, LOW);
          delay(5);
        }
}
void sag()
{
  if (Serial.available())
    {
      a = Serial.readString();
      int str_len = a.length() + 1; 
      char char_array[str_len];
      a.toCharArray(char_array, str_len);
      b = atoi(char_array);       
    }
        if(b == 1)
        {
          motor.step(-50);
          delay(5);   
        } 
  }
