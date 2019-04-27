import { Component, Input, Inject, OnChanges, SimpleChanges } from '@angular/core';
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
    selector: 'question-list',
    templateUrl: './question-list.component.html',
    styleUrls: ['./question-list.component.css']
})
/** question-list component*/
export class QuestionListComponent {
  @Input() quiz: Quiz
  questions: Question[]

    /** question-list ctor */
  constructor(private router: Router, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.questions = [];
  }


  ngOnChanges(changes: SimpleChanges) {
    if (typeof changes['quiz'] !== "undefined") {
      var change = changes['quiz'];

      if (!change.isFirstChange()) {
        this.loadData();
      }
    }
  }

  loadData() {
    var url = this.baseUrl + "api/question/All/" + this.quiz.Id;
    this.http.get<Question[]>(url).subscribe(res => {
      this.questions = res;
    }, error => console.error(error));
  }

  onCreate() {
    this.router.navigate(["/question/create", this.quiz.Id]);
  }

  onEdit(question: Question) {
    this.router.navigate(["/question/edit", question.Id]);
  }

  onDelete(question: Question) {
    if (confirm("Do you really want to delete the question?")) {
      var url = this.baseUrl + "api/question/" + question.Id;
      this.http.delete(url).subscribe(res => {
        console.log("The question {0} has been deleted.", question.Id);
        this.loadData();
      })
    }
  }
}
