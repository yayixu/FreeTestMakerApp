import { Component, Inject, Input, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";

@Component({
  selector: "quiz-list", // give a name to the html element
  templateUrl: './quiz-list.component.html',
  styleUrls: ['./quiz-list.component.css']
})                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        

export class QuizListComponent implements OnInit {
  @Input() class: string;
  title: string;
  selectedQuiz: Quiz; 
  quizzes: Quiz[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) {
  }

  ngOnInit() {
    console.log("QuizListComponent" + "instantiaated witht the following class" + this.class);
    var url = this.baseUrl + "api/quiz/";

    switch (this.class) {
      case "latest":
      default:
        this.title = "Latest Quizzes";
        url += "Latest/10";
        break;
      case "byTitle":
        this.title = "Quizzes by Tittle";
        url += "ByTitle/";
        break;
      case "random":
        this.title = "Random Quizzes";
        url += "Random/";
        break;
    }

    this.http.get<Quiz[]>(url).subscribe(result => {  // subscribe: instantiate an Observable object
      this.quizzes = result;
    }, error => console.error(error));
  }
  
  onSelect(quiz: Quiz) {
    this.selectedQuiz = quiz;
    console.log("quiz with Id" + this.selectedQuiz.Id + "has been selected.");
    this.router.navigate(["quiz", this.selectedQuiz.Id]);
  }
}


