**LLM을 통한 C# 코드 생성 및 주석 생성 Unity Plugin**

한양대학교 대학원 혼합현실 과목(2024-2)
NursingXR 의 ICPBL 산학과제 프로젝트

------------------------------------------------------------------
**Architecture**

![image](https://github.com/user-attachments/assets/8b9b0c08-0129-4691-96c7-87e2243e551e)


------------------------------------------------------------------
**How to Use**
1. LLM Tools 를 Unity Editor 내 반영하여 준비
   ![image](https://github.com/user-attachments/assets/52ad3d49-1af8-488d-bbb1-7269e7cae421)
   - 이미지와 같이, Plugin 이 보인다면 반영 된 것.


2. Model Configurator

   
   ![image](https://github.com/user-attachments/assets/e25a5e9d-992a-4f4f-aa98-ad4c4a3d8ce3)
  - 사용하기 전 OPEN AI의 API Key 와 model 설정을 하고 Save Configuration Click 

   ※ 유효한 API Key를 사용해야 원활한 이용이 가능! 

3. Code Generate
  ![image](https://github.com/user-attachments/assets/1b26db1b-8b65-4e3e-b7b2-003b0ccf7630)
   - User Prompt를 입력 하고 Code Generate 를 Click 하면, 사전 작성된 Code Generate 용  System Prompt 를 통해서 LLM API 통신
   - Requesting... 이 보이면 API 통신을 잠시 기다리기
   - LLM 응답에서 text 와 C# 코드를 분리하여 GUI 반영 및 C# 코드에 대해서는 /Generated Scripts 폴더에 추후 이용할 수 있도록 저장


4. Comment Generate
  ![image](https://github.com/user-attachments/assets/a8591d0f-c63b-4dda-b878-8998fd33c129)
  - Comment 생성을 원하는 Original file 을 선택
  - Comment Generate 를 Click 하면 사전 작성된 Comment Generate 용 System Prompt 를 통해서 LLM API 통신
  - Requesting... 이 보이면 API 통신을 잠시 기다리기
  - LLM 응답에서 C# 코드를 추출하여 GUI 반영
  - Save to Generated Scripts 를 통해 주석 코드를 따로 저장 할 수 있음
  - Apply to Original file 을 통해서 Original 파일에 주석을 반영 할 수있음

------------------------------------------------------------------
**기대 효과 및 성과**

![image](https://github.com/user-attachments/assets/28423095-88d6-42c6-8bd3-9763cfff7d08)



------------------------------------------------------------------
컴퓨터소프트웨어 학과(미래자동차 SW 융합전공) 

SELab 석사 1기 정지나

email : snowgina00@hanyang.ac.kr
