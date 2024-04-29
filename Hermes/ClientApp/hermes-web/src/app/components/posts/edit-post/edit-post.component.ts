import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { PostsService } from '../../../services/posts.service';
import { Component, OnInit, inject } from '@angular/core';
import { PostModel } from '../../../models/post';

@Component({
  selector: 'app-edit-post',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './edit-post.component.html',
  styleUrl: './edit-post.component.scss'
})
export class EditPostComponent implements OnInit {

  private readonly _postsService = inject(PostsService);
  private readonly _route = inject(ActivatedRoute);
  private readonly _router = inject(Router);

  postToEdit: PostModel | undefined;
  postId: string | undefined;

  editPostForm: FormGroup;

  constructor(){
    this.editPostForm = new FormGroup({
      'title': new FormControl(this.postToEdit?.title, Validators.required),
      'content': new FormControl(this.postToEdit?.content, Validators.required),
      'image': new FormControl(this.postToEdit?.image)
    });
  }

  ngOnInit(): void {
    this._route.params.subscribe((params: Params) => {
      this.postId = params['postId'];
      console.log(params['postId']);
      console.log(this.postId);
    })
    this._postsService.getSinglePost(this.postId!).subscribe((post)=>{
      this.postToEdit = post;
    })
  }

  onFileChange(event: any){
    if(event.target.files.length > 0){
      const file = event.target.files[0];
      this.editPostForm.patchValue({
        'image': file
      });
    }
  }

  onSubmit(){
    const formData = new FormData();
    formData.append('Post.Id', this.postId!);
    formData.append('Post.Title', this.editPostForm.get('title')?.value);
    formData.append('Post.Content', this.editPostForm.get('content')?.value);
    if(this.editPostForm.get('image')?.value){
      formData.append('Image', this.editPostForm.get('image')?.value);
    }

    this._postsService.editPost(formData).subscribe();
    //we need to redirect somewhere after post was created, at home maybe
  }

}
