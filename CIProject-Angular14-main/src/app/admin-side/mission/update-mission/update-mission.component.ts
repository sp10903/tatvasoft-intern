import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { ToastrService } from 'ngx-toastr';
import { AdminsideServiceService } from 'src/app/service/adminside-service.service';

@Component({
  selector: 'app-update-mission',
  templateUrl: './update-mission.component.html',
  styleUrls: ['./update-mission.component.css']
})
export class UpdateMissionComponent implements OnInit {
  missionId:any;
  editData:any;
  editMissionForm:FormGroup;
  formValid:boolean;
  countryList:any[]=[];
  cityList:any[]=[];
  imageUrl:any[] = [];
  missionImage : any='';
  isFileUpload = false;
  isDocUpload = false;
  missionDocName:any;
  missionDocText :any;
  formData = new FormData();
  formDoc = new FormData();
  missionThemeList:any[]=[];
  missionSkillList:any[]=[];
  typeFlag:boolean = false;
  imageListArray : any=[];
  files : any
  constructor(public fb:FormBuilder,public service:AdminsideServiceService,public toastr:ToastrService,public router:Router,public activateRoute:ActivatedRoute,
    public datePipe:DatePipe,private toast:NgToastService) {
      this.missionId = this.activateRoute.snapshot.paramMap.get("Id");
      this.editMissionForm = this.fb.group({ // Initialize editMissionForm here
          id: [''],
          missionTitle: ['', Validators.compose([Validators.required])],
          missionDescription: ['', Validators.compose([Validators.required])],
          countryId: ['', Validators.compose([Validators.required])],
          cityId: ['', Validators.compose([Validators.required])],
          startDate: ['', Validators.compose([Validators.required])],
          endDate: ['', Validators.compose([Validators.required])],
          totalSheets: [''],
          missionThemeId: ['', Validators.compose([Validators.required])],
          missionSkillId: ['', Validators.compose([Validators.required])],
          missionImages: [''],
      });
      if (this.missionId != 0) {
          this.FetchDetail(this.missionId);
      }
   }

  ngOnInit(): void {
    this.CountryList();
    this.GetMissionSkillList();
    this.GetMissionThemeList();
    this.missionDocText = '';
  }

  CountryList(){
    this.service.CountryList().subscribe((data:any)=>{
      if(data.result == 1)
      {
          this.countryList = data.data;
      }
      else
      {
          this.toast.error({detail:"ERROR",summary:data.message,duration:3000});
      }
    });
  }
    CityList(countryId:any){
      countryId = countryId.target.value;
      this.service.CityList(countryId).subscribe((data:any)=>{
      if(data.result == 1)
      {
          this.cityList = data.data;
      }
      else
      {
          this.toast.error({detail:"ERROR",summary:data.message,duration:3000});
      }
    });
  }
  HideOrShow(e:any)
  {
      if(e.target.value == "Time")
      {
        this.typeFlag = true;
      }
      else{
      this.typeFlag = false;
    }
  }
  GetMissionSkillList(){
      this.service.GetMissionSkillList().subscribe((data:any)=>{
        if(data.result==1)
        {
          this.missionSkillList = data.data;
        }
        else{
          this.toast.error({detail:"ERROR",summary:data.message,duration:3000});
        }
      },err=>this.toast.error({detail:"ERROR",summary:err.message,duration:3000}))
  }
  GetMissionThemeList(){
    this.service.GetMissionThemeList().subscribe((data:any)=>{
      if(data.result==1)
      {
        this.missionThemeList = data.data;
      }
      else{
        this.toast.error({detail:"ERROR",summary:data.message,duration:3000});
      }
    },err=>this.toast.error({detail:"ERROR",summary:err.message,duration:3000}))
  }
  FetchDetail(id:any)
  {
      this.service.MissionDetailById(id).subscribe((data:any)=>{
          this.editData = data.data;
          let startDateformat = this.datePipe.transform(this.editData.startDate,"yyyy-MM-dd");
          this.editData.startDate = startDateformat;
          let endDateformat = this.datePipe.transform(this.editData.endDate,"yyyy-MM-dd");
          this.editData.endDate = endDateformat;
          let registrationDeadLineDateformat = this.datePipe.transform(this.editData.registrationDeadLine,"yyyy-MM-dd");
          this.editData.registrationDeadLine = registrationDeadLineDateformat;
          this.editMissionForm = this.fb.group({
              id:[this.editData.id],
              missionTitle:[this.editData.missionTitle,Validators.compose([Validators.required])],
              missionDescription:[this.editData.missionDescription,Validators.compose([Validators.required])],
              countryId:[this.editData.countryId,Validators.compose([Validators.required])],
              cityId:[this.editData.cityId,Validators.compose([Validators.required])],
              startDate:[this.editData.startDate,Validators.compose([Validators.required])],
              endDate:[this.editData.endDate,Validators.compose([Validators.required])],
              totalSheets:[this.editData.totalSheets,Validators.compose([Validators.required])],
              missionThemeId:[this.editData.missionThemeId,Validators.compose([Validators.required])],
              missionSkillId:[this.editData.missionSkillId.split(','),Validators.compose([Validators.required])],
              missionImages:[''],              
          });
          this.service.CityList(this.editData.countryId).subscribe((data:any)=>{
                this.cityList = data.data;
          });
          if(this.editData.missionImages){
            let imageList = this.editData.missionImages;
            this.imageUrl = imageList.split(',');
            console.log(this.imageUrl);
            
            for (const photo of this.imageUrl) {
              this.imageListArray.push(this.service.imageUrl + '/' + photo.replaceAll('\\','/'));
            }

          }
      });
  }
  get countryId() { return this.editMissionForm.get('countryId') as FormControl; }
  get cityId() { return this.editMissionForm.get('cityId') as FormControl; }
  get missionTitle() { return this.editMissionForm.get('missionTitle') as FormControl; }
  get missionDescription() { return this.editMissionForm.get('missionDescription') as FormControl; }
  get startDate() { return this.editMissionForm.get('startDate') as FormControl; }
  get endDate() { return this.editMissionForm.get('endDate') as FormControl; }
  get missionThemeId() { return this.editMissionForm.get('missionThemeId') as FormControl; }
  get missionSkillId() { return this.editMissionForm.get('missionSkillId') as FormControl; }
  get missionImages() { return this.editMissionForm.get('missionImages') as FormControl; }


  OnSelectedImage(event:any){
    this.files = event.target.files;
    if(this.imageListArray.length > 5)
    {
      return this.toast.error({detail:"ERROR",summary:"Maximum 6 images can be added.",duration:3000});
    }
    if(this.files)
    {
      this.formData = new FormData();
      for(const file of this.files)
      {
        const reader = new FileReader();
        reader.onload = (e:any)=>{
            this.imageListArray.push(e.target.result);
        }
        reader.readAsDataURL(file)
      }
      for(let i=0;i<this.files.length;i++)
      {
          this.formData.append('file',this.files[i]);
          this.formData.append('moduleName','Mission');
      }
      this.isFileUpload = true;
    }
  }

async OnSubmit(){debugger;
  this.formValid = true;
  let missionDetail = this.editMissionForm.value;
  let updateImageUrl = '';
  let missionSkillName: any[] = [];
  if(Array.isArray(missionDetail.missionSkillId)){
    missionDetail.missionSkillId.forEach(element => {
      missionSkillName.push(this.missionSkillList.filter(e => e.value == element)[0].text);
      // missionDetail.missionSkillId = missionDetail.missionSkillId.join(',');
      missionDetail.missionSkillName = missionSkillName.join(',');
    });
  }else{
    missionSkillName.push(this.missionSkillList.filter(e => e.value == missionDetail.missionSkillId)[0].text);
    missionDetail.missionSkillName = missionSkillName[0];
  }
  var SkillLists = Array.isArray(missionDetail.missionSkillId) ? missionDetail.missionSkillId.join(",") : "";
  missionDetail.missionSkillId = SkillLists;
  console.log(this.editMissionForm)

  if(this.editMissionForm.valid)
  {
    if(this.isFileUpload){
      await this.service.UploadImage(this.formData).pipe().toPromise().then((res:any)=>{
        if(res.success){
          updateImageUrl = res.data.join(',');
        }
      },err=>this.toast.error({detail:"ERROR",summary:err.error.message}));
    }
    if(this.isFileUpload)
    {
      missionDetail.missionImages = updateImageUrl;
    }
    else
    {
      missionDetail.missionImages = this.editData.missionImages;
    }
      missionDetail.countryName = this.countryList.filter(e => e.value == missionDetail.countryId)[0].text;
      missionDetail.cityName = this.cityList.filter(e => e.value == missionDetail.cityId)[0].text;
      missionDetail.missionType = "Time";
      missionDetail.missionThemeName = this.missionThemeList.filter(e => e.value == missionDetail.missionThemeId)[0].text;
    this.service.UpdateMission(missionDetail).subscribe((data:any)=>{
          if(data.result == 1)
          {
            //this.toastr.success(data.data);
            this.toast.success({detail:"SUCCESS",summary:data.data,duration:3000});
            setTimeout(() => {
              this.router.navigate(['admin/mission']);
            }, 1000);
          }
          else
          {
            this.toastr.error(data.message);
           // this.toast.error({detail:"ERROR",summary:data.message,duration:3000});
          }
    },err=>this.toast.error({detail:"ERROR",summary:err.message,duration:3000}));
  }
}
  OnCancel()
  {
    this.router.navigateByUrl('admin/mission');
  }
  OnRemoveImage(item:any){
    const index : number = this.imageListArray.indexOf(item);
    if(item !== -1 )
    {
     this.imageListArray.splice(index,1);
     this.files.splice(index,1);
   }
  }

}
