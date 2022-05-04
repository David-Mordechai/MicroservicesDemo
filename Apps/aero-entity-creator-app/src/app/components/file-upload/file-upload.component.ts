import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
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
  errorMessage: string = '';

  get fileName() { return this.fileForm.get('fileName'); }

  get file() { return this.fileForm.get('file'); }

  constructor(private mapsService: MapsService) { }

  ngOnInit(): void {

    this.fileForm = new FormGroup({
      fileName: new FormControl('', [Validators.required]),
      file: new FormControl(''),
    });
  }

  onSelectFile(fileInput: any) {
    this.selectedFile = <File>fileInput.target.files[0];
    this.fileName?.updateValueAndValidity();
  }

  onSubmit(data: any) {
    const formData = new FormData();
    formData.append('fileName', data.fileName);
    formData.append('file', this.selectedFile);

    this.mapsService.uploadMap(formData).subscribe({
      next: response => this.onFileUploaded(response),
      error: err => console.log(err.error)
    });
  }

  onFileUploaded(response: any) {
    if (response.success) {
      this.fileForm.reset();
      this.notify.emit();
      this.errorMessage = '';
    } else {
      this.errorMessage = response.errorMessage;
      
      if(response.controlName === "File")
        this.file?.setErrors({serverError: true});

      if(response.controlName === "FileName")
        this.fileName?.setErrors({serverError: true});
    }
  }
}
