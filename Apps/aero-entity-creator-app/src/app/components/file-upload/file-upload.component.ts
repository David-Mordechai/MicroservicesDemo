import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MapsService } from 'src/app/services/maps.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {

  @Output() notify = new EventEmitter();

  selectedFile!: File;
  fileForm!: FormGroup;

  constructor(private mapsService: MapsService) { }

  ngOnInit(): void {

    this.fileForm = new FormGroup({
      fileName: new FormControl(null),
      file: new FormControl(null),
    });
  }
  
  onSelectFile(fileInput: any) {
    this.selectedFile = <File>fileInput.target.files[0];
  }
  
  onSubmit(data: any) {
    const formData = new FormData();
    formData.append('fileName', data.fileName);
    formData.append('file', this.selectedFile);
  
    this.mapsService.uploadMap(formData);
    this.fileForm.reset();
  }
}
