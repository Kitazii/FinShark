export type AppUser = {
    id: string;
    userName: string;
  };

export type CommentPost = {
    title: string;
    content: string;
};

export type CommentGet = {
    title: string;
    content: string;
    appUser: AppUser
};