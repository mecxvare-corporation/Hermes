import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { PostModel } from '../models/post';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  private basePostsUri: string = 'https://localhost:7196/api/Posts'
  private readonly _httpClient: HttpClient = inject(HttpClient);

  constructor() {}

  getAllPosts(): Observable<PostModel[]> {
    return this._httpClient.get<PostModel[]>(`${this.basePostsUri}/posts`);
  }

  getUserPosts(userId: string): Observable<PostModel[]> {
    return this._httpClient.get<PostModel[]>(`${this.basePostsUri}/user/${userId}/posts`);
  }

  getSinglePost(postId: string): Observable<PostModel> {
    return this._httpClient.get<PostModel>(`${this.basePostsUri}/post/${postId}`);
  }

  createPost(postForm: FormData): Observable<any> {
      return this._httpClient.post(`${this.basePostsUri}/create`, postForm);
  }

  editPost(postForm: FormData): Observable<any> {
    return this._httpClient.put(`${this.basePostsUri}/update`, postForm);
  }

  deletePost(postId: string): Observable<PostModel> {
    return this._httpClient.delete<PostModel>(`${this.basePostsUri}/delete/${postId}`);
  }
} 