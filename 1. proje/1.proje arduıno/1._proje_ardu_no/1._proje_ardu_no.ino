void setup() {
  pinMode(2,OUTPUT);
  pinMode(3,OUTPUT);
  pinMode(4,OUTPUT);
  pinMode(5,OUTPUT);
  pinMode(6,OUTPUT);
  pinMode(7,OUTPUT);
  pinMode(8,OUTPUT);
  pinMode(9,OUTPUT);
 pinMode(10,OUTPUT);

  Serial.begin(9600); 
}
String a;
char *c;
int b;

void loop() {
  if (Serial.available())
    {
      a = Serial.readString();
      int str_len = a.length() + 1; 
      char char_array[str_len];
      a.toCharArray(char_array, str_len);
      b = atoi(char_array);
      //Serial.println(b);
      digitalWrite(b,HIGH);
      delay (100);
      for (int i = 2; i < 11; i++)
      {
      if   (i!= b )
      {
        digitalWrite(i,LOW);
        Serial.println(i);
        }
      }
    }
      
}
