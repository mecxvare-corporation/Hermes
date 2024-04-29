import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PostsService } from '../../../../services/posts.service';
import { MatDialogRef } from '@angular/material/dialog';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-post-modal',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './post-modal.component.html',
  styleUrl: './post-modal.component.scss'
})
export class PostModalComponent {

  private readonly _postsService = inject(PostsService);
  private readonly _dialogRef = inject(MatDialogRef<PostModalComponent>)

  createPostForm: FormGroup;

  constructor(){
    this.createPostForm =  new FormGroup({
      'title': new FormControl('', Validators.required),
      'content': new FormControl('', Validators.required),
      'image': new FormControl(null)
    });
  }

  onFileChange(event: any){
    if(event.target.files.length > 0){
      const file = event.target.files[0];
      this.createPostForm.patchValue({
        'image': file
      });
    }
  }

  onSubmit(){
    const currentUserId = localStorage.getItem('currentUserId');
    if(currentUserId){
      const formData = new FormData();
      formData.append('Post.UserId', currentUserId);
      formData.append('Post.Title', this.createPostForm.get('title')?.value);
      formData.append('Post.Content', this.createPostForm.get('content')?.value);
      if(this.createPostForm.get('image')?.value){
        formData.append('Image', this.createPostForm.get('image')?.value);
      }
  
      this._postsService.createPost(formData).subscribe(() => {
        
        this._dialogRef.close();
      });
    }
  }

  onClose(){
    this._dialogRef.close();
  }
}
