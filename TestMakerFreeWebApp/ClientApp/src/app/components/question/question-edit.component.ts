import { Component, Inject } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'question-edit',
    templateUrl: './question-edit.component.html',
    styleUrls: ['./question-edit.component.css']
})
/** question-edit component*/
export class QuestionEditComponent {
  title: string;
  question: Question;
  isEditMode: boolean;
  /** question-edit ctor */
  constructor(private http: HttpClient, private router: Router, private activatedRoute: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string) {
    this.question = <Question>{};
    var id = +this.activatedRoute.snapshot.params["id"];

    this.isEditMode = this.activatedRoute.snapshot.url[1].path === "edit";

    if (this.isEditMode) {
      var url = this.baseUrl + "api/question/" + id;
      this.http.get<Question>(url).subscribe(res => {
        this.question = res;
        this.title = "Edit the question:"
      }, error => console.error(error));
    }
    else {
      this.question.QuizId = id;
      this.title = "Ctreate a new question:"
    }
  }

  onSubmit(question: Question) {
    var url = this.baseUrl + "api/question";

    if (this.isEditMode) {
      this.http.post<Question>(url, question).subscribe(res => {
        console.log("Question " + res.Id + " has been updated.");
        this.router.navigate(["quiz/edit", res.QuizId]);
      }, error => console.error(error));
    }
    else {
      this.http.put<Question>(url, question).subscribe(res => {
        console.log("Question " + res.Id + " has been created.");
        this.router.navigate(["quiz/edit", res.QuizId]);
      }, error => console.error(error)); 
    }
  }

  onBack() {
    this.router.navigate(["quiz/edit", this.question.QuizId ]);
  }
}
