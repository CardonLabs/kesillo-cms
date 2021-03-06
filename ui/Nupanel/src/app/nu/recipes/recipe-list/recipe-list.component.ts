import { Component, OnInit, Input, ViewChild, OnChanges, SimpleChanges, AfterViewInit } from '@angular/core';
import { MatTableDataSource}  from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { RecipeService } from 'src/app/nu/recipes/recipe.service';

import { Recipe, RecipeIngridient } from 'src/app/models/recipes';

@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.scss']
})
export class RecipeListComponent implements OnInit, OnChanges, AfterViewInit {

  @Input() recipeList: Array<Recipe>;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['name', 'calories', 'carbs', 'Sugars'];
  recipeTableDataSource = new MatTableDataSource<Recipe>();

  constructor(private RecipeSvc: RecipeService) { }

  ngOnInit() {
    this.recipeTableDataSource.data = this.recipeList;
  }

  ngOnChanges(changes: SimpleChanges) {
    //this.recipeTableDataSource.data = this.recipeList;
  }

  ngAfterViewInit() {
    this.recipeTableDataSource.paginator = this.paginator;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.recipeTableDataSource.filter = filterValue.trim().toLowerCase();
  }

  recipeSelect(recipe: Recipe) {
    this.RecipeSvc.selectRecipe(recipe);
  }

}
