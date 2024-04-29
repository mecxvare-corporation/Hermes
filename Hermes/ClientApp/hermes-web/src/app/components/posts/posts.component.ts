import { CreatePostComponent } from './create-post/create-post.component';
import { PostsService } from '../../services/posts.service';
import { Component, OnInit, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { PostComponent } from './post/post.component';
import { RouterOutlet } from '@angular/router';
import { PostModel } from '../../models/post';

@Component({
  selector: 'app-posts',
  standalone: true,
  imports: [ RouterOutlet, MatCardModule, PostComponent, CreatePostComponent],
  templateUrl: './posts.component.html',
  styleUrl: './posts.component.scss'
})
export class PostsComponent implements OnInit {

  posts: PostModel[] = [];

  private readonly _postsService = inject(PostsService);

  ngOnInit(): void {
    this.initializePosts();
  }

  onPostSubmitted(): void {
    this.initializePosts();
  }

  private initializePosts(): void {
    this._postsService.getAllPosts().subscribe(posts=>{
      this.posts = posts
    });
  }

  reversedPosts(): PostModel[] {
    return this.posts.slice().reverse();
  }
}
